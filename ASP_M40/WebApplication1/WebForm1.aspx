<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
</head>
<body>
    <div id="myProgress">
  <div id="myBar"></div>
</div>
    <br>
<button onclick="move()">Calculate</button> 





<script>



function move() {
  var elem = document.getElementById("myBar");   
  var width = 1;
  var id = setInterval(frame, 10);
  function frame() {
    if (width >= 100) {
      clearInterval(id);
    } else {
      width++; 
      elem.style.width = width + '%'; 
    }
  }
}





</script>


</body>
</html>




<style>
    #myProgress {
    width: 100%;
    background-color: grey;
}
#myBar {
    width: 1%;
    height: 30px;
    background-color: green;
}
</style>