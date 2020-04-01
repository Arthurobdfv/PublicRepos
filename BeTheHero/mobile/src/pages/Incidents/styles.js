import { StyleSheet } from 'react-native';
import { Contants } from 'expo-constants'

export default StyleSheet.create({
    container: {
        flex:1,
        paddingHorizontal: 24,
            paddingTop: Contants.statusBarHeight + 20,
    },

    header: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center'
    }
});