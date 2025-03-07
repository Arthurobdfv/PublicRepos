using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Debug = UnityEngine.Debug;

public class DrawOnClick : MonoBehaviour
{
    [SerializeField,Range(0,1)] float HeightScale = 0.01f;
    [SerializeField] int _textureResolution = 256;
    [SerializeField, Range(1,128)] int BrushSize = 32;
    private Texture2D _drawTexture;
    private Shader _currentShader;
    private Color[] pixels;
    private Material _currentMaterial;
    private int _drawTextureId;
    private int _normalHeightId;
    private Camera cam;
    [SerializeField] private ParticleSystemCurveMode _curve;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        _drawTexture = new Texture2D(_textureResolution,_textureResolution, textureFormat: TextureFormat.R16, false);
        _drawTexture.filterMode = FilterMode.Trilinear;
        Debug.Log($"Runtime Texture is readable: {_drawTexture.isReadable}");
        _currentMaterial = GetComponent<Renderer>().material;
        _currentShader = _currentMaterial.shader;
        _drawTextureId = _currentShader.GetPropertyNameId(_currentShader.FindPropertyIndex("_HeightMap"));
        _normalHeightId = _currentShader.GetPropertyNameId(_currentShader.FindPropertyIndex("_NormalHeight"));
        pixels = new Color[_textureResolution*_textureResolution];
        _currentMaterial.SetTexture(_drawTextureId, _drawTexture);
        _drawTexture.SetPixels(pixels);
        _drawTexture.Apply();
    
    }

    // Update is called once per frame
    void Update()
    {
        _currentMaterial.SetFloat(_normalHeightId, HeightScale);
        if(Input.GetMouseButton(0)){
            if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit))
                return;

            Renderer rend = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;

            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= _textureResolution;
            pixelUV.y *= _textureResolution;

            //_drawTexture.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.white);
            RunTimedBurstColorFunction((begin, end) => { DrawBrushInTexture(begin, end, new Vector2(pixelUV.x, pixelUV.y)); });
            _drawTexture.Apply();
        }
        if(Input.GetKeyDown(KeyCode.Z)){
            Debug.Log($"Starting {nameof(BurstUVColors)}");
            RunTimed(() => { BurstUVColors(); });
        }
        if(Input.GetKeyDown(KeyCode.X)){
            Debug.Log($"Starting {nameof(SingleThreadedColors)}");
            RunTimed(SingleThreadedColors);
        }
    }

    /// <summary>
    /// This is the Main Drawing Function
    /// </summary>
    /// <param name="beginPixel"></param>
    /// <param name="endPixel"></param>
    /// <param name="position"></param>
    private void DrawBrushInTexture(int beginPixel, int endPixel, Vector2 position){
        var squareBrushSize = BrushSize * BrushSize;
        for(int i = beginPixel; i < endPixel; i++){
            int textureX = XfromIndex(i);
            int textureY = YfromIndex(i);
            Vector2 texturePos = new Vector2(textureX, textureY);
            float sqrDistance = (texturePos - position).sqrMagnitude;
            if(sqrDistance <= squareBrushSize){
                var ratio = ((float)sqrDistance / squareBrushSize);
                var height = Mathf.Cos(ratio * Mathf.PI / 2);
                pixels[i] += Color.red * height * 0.2f;               
            }
        }
    }

    private int XfromIndex(int index) => index % _textureResolution;

    private int YfromIndex(int index) => Mathf.FloorToInt(index) / _textureResolution;

    private void RunTimed(Action function){
        var watch = new Stopwatch();
        watch.Start();
        function.Invoke();
        watch.Stop();
        Debug.Log($"Done! Elapsed: {watch.ElapsedMilliseconds}ms");
    }
    /// <summary>
    /// Function that Splits pixels evely between a number of threads depending on number of processorcounts, could easily be changed to a fixed number of jobCounts
    /// </summary>
    private void BurstUVColors(){
        var jobCount= SystemInfo.processorCount;
        var pixelsPerJob = (float)(_textureResolution*_textureResolution) / jobCount;
        var pixelsPerJobInt = Mathf.FloorToInt(pixelsPerJob);
        var threadList = new Task[jobCount];
        for(int i=0; i< jobCount; i++){
            bool isLast = i == (jobCount-1);
            int beginRange, endRange;
            beginRange = i*pixelsPerJobInt;
            endRange = isLast ? _textureResolution*_textureResolution : (i+1)*pixelsPerJobInt;
            Debug.Log($"Batch from pixels {beginRange} to {endRange} starting...");
            threadList[i] = Task.Run(() => {WritePixels(beginRange, endRange, _textureResolution);});
        }
        Task.WaitAll(threadList);
        _drawTexture.SetPixels(pixels);
        _drawTexture.Apply();
    }

    private void BurstColorFunction(Action<int, int> function){
        var jobCount= SystemInfo.processorCount;
        var pixelsPerJob = (float)(_textureResolution*_textureResolution) / jobCount;
        var pixelsPerJobInt = Mathf.FloorToInt(pixelsPerJob);
        var threadList = new Task[jobCount];
        for(int i=0; i< jobCount; i++){
            bool isLast = i == (jobCount-1);
            int beginRange, endRange;
            beginRange = i*pixelsPerJobInt;
            endRange = isLast ? _textureResolution*_textureResolution : (i+1)*pixelsPerJobInt;
            Debug.Log($"Batch from pixels {beginRange} to {endRange} starting...");
            threadList[i] = Task.Run(() => function.Invoke(beginRange, endRange));
        }
        Task.WaitAll(threadList);
        _drawTexture.SetPixels(pixels);
        _drawTexture.Apply();
    }

    private void RunTimedBurstColorFunction(Action<int, int> function){
        Debug.Log($"Starting {function.Method.Name}");
        RunTimed(() => BurstColorFunction(function));
    }

    private void SingleThreadedColors(){
        var count = 0;
        foreach(var p in pixels){
            int x = count % _textureResolution;
            int y = Mathf.FloorToInt(count / _textureResolution);
            pixels[count].r = (float)x/_textureResolution;
            pixels[count].g = (float)y/_textureResolution;
            count++;
        }
        Debug.Log($"Processed {count} of {pixels.Length} pixels");
        _drawTexture.SetPixels(pixels);
        _drawTexture.Apply();
    }

    private void WritePixels(int start, int end, int resolution){
        for(int i=start; i< end; i++){
            int count = i;
            int x = count % resolution;
            int y = Mathf.FloorToInt(count / resolution);
            pixels[count].r = (float)x/resolution;
            pixels[count].g = (float)y/resolution;
        }
    }
}
