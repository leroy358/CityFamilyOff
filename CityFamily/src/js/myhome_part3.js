window.addEventListener("load", click);

function click() {
	var decoration = document.querySelectorAll('.decoration');
	var iframeMyhomePart4 = document.querySelector('.iframeMyhomePart4');
	var part1_contentContainer3_content1_left_houseType = document.getElementById('part1_contentContainer3_content1_left_houseType');
	var scenePhoto = document.querySelectorAll('.scenePhoto');
	var timer;
	for (var i = 0; i < decoration.length; i++) {
		(function(i) {
			decoration[i].onclick = function() { //找我家，点击查看装修方案详情
				for (var j = 0; j < decoration.length; j++) {
					decoration[j].removeAttribute('decorationChoose');
				}
				for (var j = 0; j < scenePhoto.length; j++) {
					scenePhoto[j].removeAttribute('decorationChoose');
				}
				clearTimeout(timer);
				decoration[i].setAttribute('decorationChoose', '');
				part1_contentContainer3_content1_left_houseType.classList.remove('pageIn');
				part1_contentContainer3_content1_left_houseType.classList.add('pageOut');
				iframeMyhomePart4.classList.remove('pageOut');
				iframeMyhomePart4.classList.add('pageIn');
				if (decoration[i].getAttribute('is360') != null) {
					document.querySelector('.iframeMyhomePart4').style.height = 360 + 'px';
					document.querySelector('.iframeMyhomePart4').style.width = 572 + 'px';
					document.querySelector('.iframeMyhomePart4').src = decoration[i].getAttribute('is360');
					timer = setTimeout(function() {
						document.querySelector('.iframeMyhomePart4').style.height = 378 + 'px';
					}, 1000)
				} else {
					document.querySelector('.iframeMyhomePart4').src = iframeSrc1 + decoration[i].id;
				}
			}
		})(i)
	}
	for (var i = 0; i < scenePhoto.length; i++) {
		(function(i) {
			scenePhoto[i].onclick = function() { //找我家，点击查看装修方案详情
				for (var j = 0; j < scenePhoto.length; j++) {
					scenePhoto[j].removeAttribute('decorationChoose');
				}
				for (var j = 0; j < decoration.length; j++) {
					decoration[j].removeAttribute('decorationChoose');
				}
				scenePhoto[i].setAttribute('decorationChoose', '');
				part1_contentContainer3_content1_left_houseType.classList.remove('pageIn');
				part1_contentContainer3_content1_left_houseType.classList.add('pageOut');
				iframeMyhomePart4.classList.remove('pageOut');
				iframeMyhomePart4.classList.add('pageIn');

				document.querySelector('.iframeMyhomePart4').src = iframeSrc2 + scenePhoto[i].id;
			}
		})(i)
	}
	var houseTypeAnalyseContainer = document.querySelectorAll('.houseTypeAnalyseContainer');
	for (var i = 0; i < houseTypeAnalyseContainer.length; i++) {
		houseTypeAnalyseContainer[i].onclick = function() {
			for (var j = 0; j < scenePhoto.length; j++) {
				scenePhoto[j].removeAttribute('decorationChoose');
			}
			for (var j = 0; j < decoration.length; j++) {
				decoration[j].removeAttribute('decorationChoose');
			}
			iframeMyhomePart4.classList.remove('pageIn');
			iframeMyhomePart4.classList.add('pageOut');
			part1_contentContainer3_content1_left_houseType.classList.remove('pageOut');
			part1_contentContainer3_content1_left_houseType.classList.add('pageIn');
		}
	}
}