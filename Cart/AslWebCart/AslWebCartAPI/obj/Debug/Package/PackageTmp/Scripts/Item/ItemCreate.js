$('#SubmitId').click(function () {

    var itemName = $("#itemName").val();

    var stockType = $("#stockType").val();
    var rateBDT = $("#rateBDT").val();

    var rateUSD = $("#rateUSD").val();
    $.ajax({
        url: '/api/ItemCreate/',
        type: 'GET',
        cache: false,
        data: { ItemName: itemName, StockType: stockType, RateBDT: rateBDT, RateUSD: rateUSD },
        dataType: 'json',
        success: function(data) {


            alert("Saved");


        }
        
});

    $("#itemName").val("");
    $("#stockType").val("");
    $("#rateBDT").val("");
    $("#rateUSD").val("");
    return true;
});