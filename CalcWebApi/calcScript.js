$(document).ready(function(){

    $('#submit').click(function () {
        var firstArg = $('#FirstArg').val();
        var secondArg = $('#SecondArg').val();
        var operator = $('input:radio[name=Operator]:checked').val();  // Get value of selected radio button.

        $.getJSON("/api/calc", { FirstArg: firstArg, SecondArg: secondArg, Operator: operator })
            .done(function (data) {
                $('#Message').html(data);
                var dataArray = data.split(',');   // Not returning complicated JSON here, able to split on the comma.
                $('#FirstArg').val(dataArray[0]);  // Put the answer up into the first argument text box.
                $('#SecondArg').val('');           // Clear out the value in the second arguement text box.
            });
    });

});