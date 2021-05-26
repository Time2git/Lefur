////// Write your JavaScript code.
////var i = 0;
////var arr = new Map();


////function addInput() {
////    var str = '<div class="d-flex justify-content-around row" id = ' + i + '><textarea class="form-control mt-2 mb-3 ml-3 mr-3" name = "Questions[' + i + '].Context" rows="1" style="resize:none"></textarea><button class="btn btn-outline-dark col-4 align-content-center" id="button' + i + '" onclick="addAnswer(this)" type="button">Добавить ответ</button></div>'
    
////    butt.insertAdjacentHTML('beforebegin', str);
////    i++;
////}

////function addAnswer(val) {
////    let x;
////    //alert(val);
////    //alert(val.parentNode.id);
////    let listId = val.parentNode.id;
////    /*alert(listId);*/
////    if (arr.has(listId)) {
////        x = arr.get(listId);
////        x += 1;
////        arr.set(listId, x)
////    }
////    else {
////        x = 0;
////        arr.set(listId, x)
////    }
////    var str = '<div class="form-check col-sm-4"><input class="form-check-input" type="checkbox" id = Questions[' + listId + '].Answers[' + x + '].State name = Questions[' + listId + '].Answers[' + x + '].State value = "true"><input class="form-control ml-2 mb-2" name = Questions[' + listId + '].Answers[' + x +'].Content></div >'
////    val.insertAdjacentHTML('beforebegin', str);
////}