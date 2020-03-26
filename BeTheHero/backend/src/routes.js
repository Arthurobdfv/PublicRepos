const express = require('express');
const ongController = require('./controller/OngController')
const incidentController = require('./controller/incidentController');
const ProfileController = require('./controller/ProfileController');
const SessionController = require('./controller/sessionController');

const routes = express.Router();

routes.post('/ongs', ongController.create);
routes.get('/ongs', ongController.index);

routes.post('/incidents', incidentController.create);
routes.get('/incidents', incidentController.index);
routes.delete('/incidents/:id', incidentController.delete);

routes.get('/profile', ProfileController.index);

routes.post('/sessions', SessionController.create);

module.exports = routes;
