function formatDate(value)
{
  var month = parseInt(value.getMonth()) + 1;
  if(month < 10)
  {
    month = "0" + month;
  }

  return value.getFullYear() + "-" + month + "-" + value.getDate();
}

$(function(){
  $('input[type="date"][value="now"]').each(function(){
        var date = new Date();
    $(this).attr({
      'value': formatDate(date)
    });
  });
});
