   

    $('#categoryName').autocomplete({
        source: function (request, response) {
            $.ajax({
                   

                url: '/api/categorylist',
                type: 'GET',
                cache: false,
                data: { query: request.term },
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.CategoryName,
                            value: item.CategoryName,
                            x:item.CategoryId,
                            levelvalue:item.LevelGroup
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#categoryName').val(ui.item.label);
            $('#categoryID').val(ui.item.x);
            $('#levelGroup').val(ui.item.levelvalue);
                
            //return false;
        },
        change: $('#categoryName').keyup(_.debounce(txtOneChanged, 1000))
           
        //minLength: 1
    });

function txtOneChanged() {


    var changedText = $("#categoryName").val();
    var changedText2 = $("#categoryID").val();
    
    var Name = document.getElementById('categoryName');
    var txtBox = document.getElementById('categoryID');
    var txtBox2 = document.getElementById('levelGroup');

         

    if (changedText != "") {

        $.getJSON(
            '/api/Dynamicautocomplete', { "ChangedText": changedText, "ChangedText2": changedText2 },
            function (result) {
                Name.value = result[0].CategoryName;
                txtBox.value = result[0].CategoryId;
                txtBox2.value = result[0].LevelGroup;
                document.getElementById("categoryName").focus();
            });
    }



}


$('#SubmitId').click(function () {

    var categoryName = $("#categoryName").val();

    var levelGroup = $("#levelGroup").val();
    var id = $("#categoryID").val();
    $.ajax({
        url: '/api/CategoryUpdate/',
        type: 'GET',
        cache: false,
        data: { CategoryName: categoryName, LevelGroup: levelGroup ,Id:id},
        dataType: 'json',
        success: function (data) {


            alert("Updated");


        }

    });

    $("#categoryID").val("");
    $("#categoryName").val("");
    $("#levelGroup").val("");

});

