// window.addEventListener("load", menuItem1);
var iframeH;

function menuItem1() {
	var partContainer = document.querySelectorAll('.partContainer');
	var part1_contentContainer1 = document.getElementById('part1_contentContainer1');
	var part1_contentContainer2 = document.getElementById('part1_contentContainer2');
	houseType()
}

function houseType() {
	var houseType_all = document.querySelectorAll('.houseType_all');
	var decoration = document.querySelectorAll('.decoration');
	var part1_contentContainer3_content1_left_houseType = document.getElementById('part1_contentContainer3_content1_left_houseType');
	var part1_contentContainer3_content1_left_decoration = document.getElementById('part1_contentContainer3_content1_left_decoration');

	var part1_contentContainer3_content1_left_topDecoration = document.querySelectorAll('.part1_contentContainer3_content1_left_topDecoration');
	var changeDecorationPageContainer = document.getElementById('changeDecorationPageContainer');
	var pageNum = -1;
	var currentPage = 0;
	var xx = simple.preloader.create();
	xx.concurrentLoad = false;
	changeDecorationPageContainer.innerHTML = ''; //先把切换页数的按钮组件清空
	// var part1_contentContainer3_content1_left_topPics = document.getElementById('part1_contentContainer3_content1_left_topPics');
	// part1_contentContainer3_content1_left_topPics.classList.remove('pageIn');
	// part1_contentContainer3_content1_left_topPics.classList.add('pageOut');
	document.getElementById('loading').classList.remove('pageOut');
	document.getElementById('loading').classList.add('pageIn');
	for (var i = 0; i < part1_contentContainer3_content1_left_topDecoration.length; i++) {
		part1_contentContainer3_content1_left_topDecoration[i].classList.remove('pageIn');
		part1_contentContainer3_content1_left_topDecoration[i].classList.add('pageOut');
		xx.addItem(part1_contentContainer3_content1_left_topDecoration[i].src);
	}
	xx.onComplete = function() {
		document.getElementById('loading').classList.remove('pageIn');
		document.getElementById('loading').classList.add('pageOut');
		// part1_contentContainer3_content1_left_topPics.classList.add('pageIn');
		// part1_contentContainer3_content1_left_topPics.classList.remove('pageOut');
		part1_contentContainer3_content1_left_topDecoration[0].classList.remove('pageOut');
		part1_contentContainer3_content1_left_topDecoration[0].classList.add('pageIn');
		startShow();
	}

	xx.start();

	function startShow() {
		for (var j = 0; j < part1_contentContainer3_content1_left_topDecoration.length + 2; j++) {
			pageNum++;
			var changeDecorationPage = document.createElement('div');
			changeDecorationPageContainer.appendChild(changeDecorationPage);
			changeDecorationPage.setAttribute('class', 'changeDecorationPage');
			changeDecorationPage.innerHTML = pageNum;;
		}
		document.querySelectorAll('.changeDecorationPage')[0].innerHTML = '上一张';
		document.querySelectorAll('.changeDecorationPage')[part1_contentContainer3_content1_left_topDecoration.length + 2 - 1].innerHTML = '下一张';
		if (part1_contentContainer3_content1_left_topDecoration.length < 13) {
			changeDecorationPageContainer.style.left = (572 - 34 * (part1_contentContainer3_content1_left_topDecoration.length) - 65 * 2) / 2 - 12 + 'px';
		}
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
		iframeH = part1_contentContainer3_content1_left_decoration.offsetHeight;
		document.height = part1_contentContainer3_content1_left_decoration.offsetHeight + 'px';
		window.parent.document.querySelector('.iframeMyhomePart4').style.height = part1_contentContainer3_content1_left_decoration.offsetHeight + 'px';
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

}