function formatDate(value)
{
   return value.getFullYear() + "-" + value.getMonth()+1 + "-" + value.getDate();
}

$(function(){
  $('input[type="date"][value="now"]').each(function(){
        var date = new Date();
    $(this).attr({
      'value': formatDate(date)
    });
  });
});
