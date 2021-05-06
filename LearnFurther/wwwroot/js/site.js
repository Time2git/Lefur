// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var x = 0;

function addInput() {
    /*var str = '<input type="text" class="link" id = "inputOne' + (One + 1) + '" placeholder="Ссылка на профиль *"> <input type="text" class="amount" id = "inputTwo' + (Two + 1) + '" placeholder="Кол-во"> <div id="input' + (x + 1) + '"></div>';*/
    var str = '<textarea class="form-control mt-2 mb-3" id = "question' + x + '" rows="1" style="resize:none"></textarea>'
    butt.insertAdjacentHTML('beforebegin', str);
    x++;
}