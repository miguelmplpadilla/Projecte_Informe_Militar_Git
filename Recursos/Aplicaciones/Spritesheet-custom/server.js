const path = require('path');
const express = require('express');
const { log } = require('console');
const app = express();
const fs = require('fs');
const { arch } = require('os');

app.engine('html', require('ejs').renderFile); 

app.use(express.static(__dirname + '/public'));

var insercionHtml = '';

var pathInicial = './public/spritesheets';

var files = fs.readdirSync(pathInicial);

for (var i = 0; i < files.length; i++) {
    console.log(files[i]);

    crearCategoria('./public/spritesheets/', files[i]);
}


app.get('/', function(req, res) {
    //res.sendFile(path.join(__dirname + '/index.html'));
    res.render(__dirname + '/index.html', {name:insercionHtml});
});

app.listen(8080);

console.log("Server runing");

function crearCategoria(path, tipo) {
    var pathBody = path+tipo;

    var files = fs.readdirSync(pathBody);

    insercionHtml = insercionHtml + '<li> <span class="condensed"> '+tipo+' </span> <ul>';

    //insercionHtml = insercionHtml + '<li><input type="radio" id="'+tipo+'-no_'+tipo+'" name="'+tipo+'" data-file="/1-body/none.png"><label for="'+tipo+'-no_'+tipo+'">None</label></li>';

    for (var i = 0; i < files.length; i++) {
        
        if (fs.statSync(pathBody+'/'+files[i]).isDirectory()) {
            insercionHtml = insercionHtml + '<li class="'+files[i]+'"> <span class="condensed">'+files[i]+'</span> <ul>';

            var archivos = fs.readdirSync(pathBody+"/"+files[i]);

            for (var j = 0; j < archivos.length; j++) {

                console.log(archivos[j]);

                var pathArchivo = pathBody+"/"+files[i]+"/"+archivos[j];

                if (!fs.statSync(pathArchivo).isDirectory()) {
                    console.log("Creando lista");
                    insercionHtml = addDomToList(archivos[j], pathArchivo, insercionHtml, tipo, files[i]);
                } else {
                    console.log("Creando carpeta");
                    crearCategoria(pathBody+'/'+files[i]+'/', archivos[j]);
                }
            }
        
            insercionHtml = insercionHtml + '</ul> </li>';
        } else {
            var pathArchivo = pathBody+"/"+files[i];
            insercionHtml = addDomToList(files[i], pathArchivo, insercionHtml, tipo, files[i]);
        }
    }

    insercionHtml = insercionHtml + '</ul> </li>';
}

function addDomToList(nombreArchivo, path, html, tipo, carpeta) {
    return html + '<li> <input type="radio" id="'+tipo+'-'+carpeta+'_'+nombreArchivo.replace(".png", "")+'" name="'+tipo+'" data-file="'+path.replace("./public/spritesheets","")+'"> <label for="'+tipo+'-'+nombreArchivo+'">'+nombreArchivo+'</label> </li>';
}