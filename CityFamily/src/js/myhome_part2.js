window.addEventListener("load", menuItem1);
window.addEventListener("load", lookFacility);
function menuItem1() {
	var partContainer = document.querySelectorAll('.partContainer');
	houseType();
	changePics();
}

function changePics() {
	var part1_contentContainer2_content1_left_pic = document.getElementById('part1_contentContainer2_content1_left_pic');
	var bannerDots = document.getElementById('bannerDots');
	var images = document.querySelectorAll('.images');
	var timer1;
	var currentNumber = 0;
	bannerDots.innerHTML = '';
	for (var i = 0; i < images.length; i++) {
		var dots = document.createElement('div');
		bannerDots.appendChild(dots);
		dots.setAttribute('class', 'bannerdot');
	}
	var bannerdot = document.querySelectorAll('.bannerdot');
	bannerdot[0].setAttribute('current', '');
	images[0].setAttribute('current', '');

	function move() {
		removeCurrent();
		currentNumber++;
		if (currentNumber >= images.length) {
			currentNumber = 0;
		}
		images[currentNumber].setAttribute('current', '');
		bannerdot[currentNumber].setAttribute('current', '');
	};

	function removeCurrent() {
		images[currentNumber].removeAttribute('current');
		bannerdot[currentNumber].removeAttribute('current');
	}
	for (var i = 0; i < images.length; i++) {
		bannerdot[i].index = i;
	}
	for (var i = 0; i < images.length; i++) {
		bannerdot[i].index = i;
		bannerdot[i].onclick = function() {
			clearInterval(timer1);
			currentNumber = this.index;
			for (var j = 0; j < images.length; j++) {
				images[j].removeAttribute('current');
				bannerdot[j].removeAttribute('current');
			}
			images[currentNumber].setAttribute('current', '');
			bannerdot[currentNumber].setAttribute('current', '');
			timer1 = setInterval(move, 3000);
		}
	}
	timer1 = setInterval(move, 3000);
};

function houseType() {
	var houseType_all = document.querySelectorAll('.houseType_all');
	var decoration = document.querySelectorAll('.decoration');
	var houseType_width = 232;
	var houseTypeContainer_in = document.getElementById('houseTypeContainer_in');
	var houseTypeContainer_out = document.getElementById('houseTypeContainer_out');
	houseTypeContainer_in.style.width = houseType_all.length * (houseType_width + 2) + 'px';
	var houseTypeChange_next = document.getElementById('houseTypeChange_next');
	var houseTypeChange_previous = document.getElementById('houseTypeChange_previous');
	var part1_contentContainer3_content1_left_houseType = document.getElementById('part1_contentContainer3_content1_left_houseType');
	var part1_contentContainer3_content1_left_decoration = document.getElementById('part1_contentContainer3_content1_left_decoration');

	houseTypeChange_next.onclick = function() {
		if (houseTypeContainer_in.offsetWidth > houseTypeContainer_out.offsetWidth) {

			var next = simple.tweener.create(houseTypeContainer_in.offsetLeft, houseTypeContainer_in.offsetLeft - (houseType_width + 2), 600, moveleft, "easeOutQuart");

			function moveleft() {
				houseTypeContainer_in.style.left = this.getCurrentValue() + "px";
				if (houseTypeContainer_in.offsetLeft <= houseTypeContainer_out.offsetWidth - houseTypeContainer_in.offsetWidth) {
					houseTypeContainer_in.style.left = houseTypeContainer_out.offsetWidth - houseTypeContainer_in.offsetWidth + 'px';
				}
			}
			next.start();
		}
	}
	houseTypeChange_previous.onclick = function() {
		if (houseTypeContainer_in.offsetWidth > houseTypeContainer_out.offsetWidth) {
			var previous = simple.tweener.create(houseTypeContainer_in.offsetLeft, houseTypeContainer_in.offsetLeft + (houseType_width + 2), 600, moveright, "easeOutQuart");

			function moveright() {
				houseTypeContainer_in.style.left = this.getCurrentValue() + "px";
				if (houseTypeContainer_in.offsetLeft >= 0) {
					houseTypeContainer_in.style.left = 0 + 'px';
				}
			}
			previous.start();
		}
	}
	for (var i = 0; i < decoration.length; i++) {
		(function(i) {
			decoration[i].onclick = function() { //找我家，点击查看装修方案详情
				for (var j = 0; j < decoration.length; j++) {
					decoration[j].removeAttribute('decorationChoose');
				}
				decoration[i].setAttribute('decorationChoose', '');
				part1_contentContainer3_content1_left_houseType.classList.remove('pageIn');
				part1_contentContainer3_content1_left_houseType.classList.add('pageOut');
				part1_contentContainer3_content1_left_decoration.classList.remove('pageOut');
				part1_contentContainer3_content1_left_decoration.classList.add('pageIn');
				var part1_contentContainer3_content1_left_topDecoration = document.querySelectorAll('.part1_contentContainer3_content1_left_topDecoration');
				var changeDecorationPageContainer = document.getElementById('changeDecorationPageContainer');
				var pageNum = -1;
				var currentPage = 0;
				changeDecorationPageContainer.innerHTML = ''; //先把切换页数的按钮组件清空
				for (var j = 0; j < part1_contentContainer3_content1_left_topDecoration.length + 2; j++) {
					pageNum++;
					var changeDecorationPage = document.createElement('div');
					changeDecorationPageContainer.appendChild(changeDecorationPage);
					changeDecorationPage.setAttribute('class', 'changeDecorationPage');
					changeDecorationPage.innerHTML = pageNum;;
				}
				document.querySelectorAll('.changeDecorationPage')[0].innerHTML = '上一张';
				document.querySelectorAll('.changeDecorationPage')[part1_contentContainer3_content1_left_topDecoration.length + 2 - 1].innerHTML = '下一张';
				changeDecorationPageContainer.style.left = (572 - 34 * (part1_contentContainer3_content1_left_topDecoration.length) - 65 * 2) / 2 + 'px';
				var changeDecorationPage = document.querySelectorAll('.changeDecorationPage');
				changeDecorationPage[1].setAttribute('current_page', '');
				for (var x = 1; x < changeDecorationPage.length - 1; x++) {
					(function(x) {
						changeDecorationPage[x].onclick = function() {
							currentPage = x - 1;
							for (var ii = 0; ii < part1_contentContainer3_content1_left_topDecoration.length; ii++) {
								part1_contentContainer3_content1_left_topDecoration[ii].classList.remove('pageIn');
								part1_contentContainer3_content1_left_topDecoration[ii].classList.add('pageOut');
								part1_contentContainer3_content1_left_topDecoration[currentPage].classList.add('pageIn');
								part1_contentContainer3_content1_left_topDecoration[currentPage].classList.remove('pageOut');
								changeDecorationPage[ii + 1].removeAttribute('current_page');
								changeDecorationPage[x].setAttribute('current_page', '');
							}
						}
					})(x)
				}
				changeDecorationPage[changeDecorationPage.length - 1].onclick = function() { //点击下一张
					currentPage++;
					if (currentPage > part1_contentContainer3_content1_left_topDecoration.length - 1) {
						currentPage = part1_contentContainer3_content1_left_topDecoration.length - 1;
					}
					for (var ii = 0; ii < part1_contentContainer3_content1_left_topDecoration.length; ii++) {
						part1_contentContainer3_content1_left_topDecoration[ii].classList.remove('pageIn');
						part1_contentContainer3_content1_left_topDecoration[ii].classList.add('pageOut');
						part1_contentContainer3_content1_left_topDecoration[currentPage].classList.add('pageIn');
						part1_contentContainer3_content1_left_topDecoration[currentPage].classList.remove('pageOut');
						changeDecorationPage[ii + 1].removeAttribute('current_page');
						changeDecorationPage[currentPage + 1].setAttribute('current_page', '');
					}
				}
				changeDecorationPage[0].onclick = function() { //点击上一张
					currentPage--;
					if (currentPage < 0) {
						currentPage = 0;
					}
					for (var ii = 0; ii < part1_contentContainer3_content1_left_topDecoration.length; ii++) {
						part1_contentContainer3_content1_left_topDecoration[ii].classList.remove('pageIn');
						part1_contentContainer3_content1_left_topDecoration[ii].classList.add('pageOut');
						part1_contentContainer3_content1_left_topDecoration[currentPage].classList.add('pageIn');
						part1_contentContainer3_content1_left_topDecoration[currentPage].classList.remove('pageOut');
						changeDecorationPage[ii + 1].removeAttribute('current_page');
						changeDecorationPage[currentPage + 1].setAttribute('current_page', '');
					}
				}
			}
		})(i)
	}
}
function lookFacility(){
	var facility=document.getElementById('facility');
	var lookFacility=document.getElementById('lookFacility');
	var closeFacilityPic=document.getElementById('closeFacilityPic');
	lookFacility.onclick=function(){
		facility.classList.remove('pageOut');
		facility.classList.add('pageIn');
	}
	closeFacilityPic.onclick=function(){
		facility.classList.remove('pageIn');
		facility.classList.add('pageOut');
	}
}