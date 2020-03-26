const express = require('express');
const routes = require('./routes');
const cors = require('cors');

const app = express();
app.use(cors());
app.use(express.json());
app.use(routes);

/**
 * Metodos HTTP
 * 
 * GET: Buscar/Listar uma informação do back-end
 * POST: Criar uma informação no back-end
 * PUT: Alterar uma informação no back end
 * DELETE: Deletar uma informação no back-end
 */

 /**
  * Tiposde parâmetros:
  * 
  * Query Params: Parametros nomeados enviados na rota após "?" (filtro, paginação)
  * Route Params: Parametros utilizados para identificar recursos 
  * Request Body: Corpo da requisição, utilizado para criar ou alterar dados
  */

  /**
   * SQL: MySQL , SQLite, PostgreSQL, Oracle, Microsoft SQL Server
   * NoSQL: MongoDB, CouchDB, etc
   */

   /**
    * Driver: SELECT * FROM users
    * Query Builder: table('users').select('*').where(condition)
    */

app.listen(3333);