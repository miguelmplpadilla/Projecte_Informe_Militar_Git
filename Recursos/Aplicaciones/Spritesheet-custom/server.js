const path = require('path');
const express = require('express');
const { log } = require('console');
const app = express();
const fs = require('fs');
const { arch } = require('os');

app.engine('html', require('ejs').renderFile); 

app.use(express.static(__dirname + '/public'));

var insercion = crearCategoria("body");

app.get('/', function(req, res) {
    //res.sendFile(path.join(__dirname + '/index.html'));
    res.render(__dirname + '/index.html', {name:insercion});
});

app.listen(8080);

console.log("Server runing");

function crearCategoria(tipo) {
    var pathBody = './public/spritesheets/'+tipo;

    var files = fs.readdirSync(pathBody);

    var insercionHtml1 = '<li> <span class="condensed"> Color Piel </span> <ul>';

    insercionHtml1 = insercionHtml1 + '<li><input type="radio" id="'+tipo+'-no_'+tipo+'" name="'+tipo+'" data-file="'+tipo+'/none.png"><label for="'+tipo+'-no_'+tipo+'">None</label></li>';

    for (var i = 0; i < files.length; i++) {
        if (!files[i].includes(".png")) {
            insercionHtml1 = insercionHtml1 + '<li> <span class="condensed">'+files[i]+'</span> <ul>';

            var archivos = fs.readdirSync(pathBody+"/"+files[i]);

            for (var j = 0; j < archivos.length; j++) {
                insercionHtml1 = addDomToList(archivos[j], pathBody+"/"+files[i]+"/"+archivos[j], insercionHtml1, tipo);
            }
        
            insercionHtml1 = insercionHtml1 + '</ul> </li>';
        }
    }

    insercionHtml1 = insercionHtml1 + '</ul> </li>';

    return insercionHtml1;
}

function addDomToList(nombreArchivo, path, html, tipo) {
    return html + '<li> <input type="radio" id="'+tipo+'-'+nombreArchivo+'" name="'+tipo+'" data-file="'+path.replace("./public/spritesheets","")+'"> <label for="'+tipo+'-'+nombreArchivo+'">'+nombreArchivo+'</label> </li>';
}