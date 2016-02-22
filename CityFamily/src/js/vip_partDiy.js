window.addEventListener("load", menuItem4);

function menuItem4() {
	var part4_contentContainer2 = document.querySelector('.part4_contentContainer2');
	var part4_item = document.getElementById('part4_contentContainer1').querySelectorAll('.part3_item');
	var part4_vipContainer = part4_contentContainer2.querySelectorAll('.part4_vipContainer');
	var part4_vipBackground = part4_contentContainer2.querySelectorAll('.part4_vipBackground');
	var vipCloseDiy = part4_contentContainer2.querySelectorAll('.vipClose');
	for (var i = 0; i < vipCloseDiy.length; i++) {
		(function(i) {
			vipCloseDiy[i].onclick = function() {
				var xmlhttp = new XMLHttpRequest();
				xmlhttp.onreadystatechange = function() {
					if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
						if (xmlhttp.responseText == 'Success') {
							part4_contentContainer2.removeChild(part4_vipContainer[i]);
						} else if (xmlhttp.responseText == 'Error') {
							alert('删除失败!');
						}
					}
				}
				xmlhttp.open("POST", closeUrlDiy, true);
				xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
				xmlhttp.send(part4_vipContainer[i].id);
			}
		})(i)
	}
}