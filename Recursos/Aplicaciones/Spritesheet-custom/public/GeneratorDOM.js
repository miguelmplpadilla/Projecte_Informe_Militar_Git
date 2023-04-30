var stringToHTML = function (str) {
    var dom = document.createElement('li');
    dom.innerHTML = str;
    return dom;
};

var ul = document.getElementById("pruebaInsercion");
var append = "<%= name %>";

var html = stringToHTML(append);

ul.innerHTML =  html.textContent + ul.innerHTML;