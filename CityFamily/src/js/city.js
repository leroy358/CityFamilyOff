window.addEventListener("load", menuItem1);
window.addEventListener("load", menuItem2);
window.addEventListener("load", menuItem3);
window.addEventListener("load", menuItem4);

function menuItem1() {
	var menuItem = document.querySelectorAll('.menuItem');
	var partContainer = document.querySelectorAll('.partContainer');
	menuItem[0].onclick = function() {
		for (var i = 0; i < partContainer.length; i++) {
			partContainer[i].classList.remove('pageIn');
			partContainer[i].classList.add('pageOut');
			menuItem[i].classList.remove('choosed');
		}
		menuItem[0].classList.add('choosed');
		partContainer[0].classList.remove('pageOut');
		partContainer[0].classList.add('pageIn');
	}
	var bulidingContainer = document.querySelectorAll('.bulidingContainer');
	var part1_contentContainer1 = document.getElementById('part1_contentContainer1');
	var part1_contentContainer2 = document.getElementById('part1_contentContainer2');

	for (var i = 0; i < bulidingContainer.length; i++) {
		(function(i) {
			bulidingContainer[i].onclick = function() { //找我家，点击查看详细楼盘
				houseType();
				changePics();
				part1_contentContainer1.classList.remove('pageIn');
				part1_contentContainer1.classList.add('pageOut');
				part1_contentContainer2.classList.remove('pageOut');
				part1_contentContainer2.classList.add('pageIn');
			}
		})(i)
	}
	part1_contentContainer2.querySelectorAll('.pathTipItem')[0].onclick = function() {
		part1_contentContainer1.classList.remove('pageOut');
		part1_contentContainer1.classList.add('pageIn');
		part1_contentContainer2.classList.remove('pageIn');
		part1_contentContainer2.classList.add('pageOut');
	}
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
	for (var i = 0; i < houseType_all.length; i++) {
		(function(i) {
			houseType_all[i].onclick = function() { //找我家，点击查看详细户型
				part1_contentContainer2.classList.remove('pageIn');
				part1_contentContainer2.classList.add('pageOut');
				part1_contentContainer3.classList.remove('pageOut');
				part1_contentContainer3.classList.add('pageIn');
				part1_contentContainer3_content1_left_houseType.classList.remove('pageOut');
				part1_contentContainer3_content1_left_houseType.classList.add('pageIn');
				part1_contentContainer3_content1_left_decoration.classList.remove('pageIn');
				part1_contentContainer3_content1_left_decoration.classList.add('pageOut');
				for (var j = 0; j < decoration.length; j++) {
					decoration[j].removeAttribute('decorationChoose');
				}
			}
		})(i)
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
	part1_contentContainer3.querySelectorAll('.pathTipItem')[0].onclick = function() {
		part1_contentContainer1.classList.remove('pageOut');
		part1_contentContainer1.classList.add('pageIn');
		part1_contentContainer3.classList.remove('pageIn');
		part1_contentContainer3.classList.add('pageOut');
	}
	part1_contentContainer3.querySelectorAll('.pathTipItem')[1].onclick = function() {
		part1_contentContainer2.classList.remove('pageOut');
		part1_contentContainer2.classList.add('pageIn');
		part1_contentContainer3.classList.remove('pageIn');
		part1_contentContainer3.classList.add('pageOut');
	}
	part1_contentContainer3.querySelectorAll('.pathTipItem')[2].onclick = function() {
		part1_contentContainer3_content1_left_houseType.classList.remove('pageOut');
		part1_contentContainer3_content1_left_houseType.classList.add('pageIn');
		part1_contentContainer3_content1_left_decoration.classList.remove('pageIn');
		part1_contentContainer3_content1_left_decoration.classList.add('pageOut');
	}
}

function menuItem2() {
	var menuItem = document.querySelectorAll('.menuItem');
	var partContainer = document.querySelectorAll('.partContainer');
	menuItem[1].onclick = function() {
		for (var i = 0; i < partContainer.length; i++) {
			partContainer[i].classList.remove('pageIn');
			partContainer[i].classList.add('pageOut');
			menuItem[i].classList.remove('choosed');
		}
		menuItem[1].classList.add('choosed');
		partContainer[1].classList.remove('pageOut');
		partContainer[1].classList.add('pageIn');
	}
	var styleContainer = document.querySelectorAll('.styleContainer');
	for (var i = 0; i < styleContainer.length; i++) {
		(function(i) {
			styleContainer[i].onclick = function() { //风格定位，点击查看八大详细风格
				part2_contentContainer1.classList.remove('pageIn');
				part2_contentContainer1.classList.add('pageOut');
				part2_contentContainer2.classList.remove('pageOut');
				part2_contentContainer2.classList.add('pageIn');
			}
		})(i)
	}
	part2_contentContainer2.querySelectorAll('.pathTipItem')[0].onclick = function() {
		part2_contentContainer1.classList.remove('pageOut');
		part2_contentContainer1.classList.add('pageIn');
		part2_contentContainer2.classList.remove('pageIn');
		part2_contentContainer2.classList.add('pageOut');
	}
	var specific_styleContainer = document.querySelectorAll('.specific_styleContainer');
	for (var i = 0; i < specific_styleContainer.length; i++) {
		(function(i) {
			specific_styleContainer[i].onclick = function() {
				part2_contentContainer2.classList.remove('pageIn');
				part2_contentContainer2.classList.add('pageOut');
				part2_contentContainer3.classList.remove('pageOut');
				part2_contentContainer3.classList.add('pageIn');
			}
		})(i)
	}
	part2_contentContainer3.querySelectorAll('.pathTipItem')[0].onclick = function() {
		part2_contentContainer1.classList.remove('pageOut');
		part2_contentContainer1.classList.add('pageIn');
		part2_contentContainer3.classList.remove('pageIn');
		part2_contentContainer3.classList.add('pageOut');
	}
	part2_contentContainer3.querySelectorAll('.pathTipItem')[1].onclick = function() {
		part2_contentContainer2.classList.remove('pageOut');
		part2_contentContainer2.classList.add('pageIn');
		part2_contentContainer3.classList.remove('pageIn');
		part2_contentContainer3.classList.add('pageOut');
	}
	part2_contentContainer4.querySelectorAll('.pathTipItem')[0].onclick = function() {
		part2_contentContainer1.classList.remove('pageOut');
		part2_contentContainer1.classList.add('pageIn');
		part2_contentContainer4.classList.remove('pageIn');
		part2_contentContainer4.classList.add('pageOut');
	}
	part2_contentContainer4.querySelectorAll('.pathTipItem')[1].onclick = function() {
		part2_contentContainer2.classList.remove('pageOut');
		part2_contentContainer2.classList.add('pageIn');
		part2_contentContainer4.classList.remove('pageIn');
		part2_contentContainer4.classList.add('pageOut');
	}
	part2_contentContainer4.querySelectorAll('.pathTipItem')[2].onclick = function() {
		part2_contentContainer3.classList.remove('pageOut');
		part2_contentContainer3.classList.add('pageIn');
		part2_contentContainer4.classList.remove('pageIn');
		part2_contentContainer4.classList.add('pageOut');
	}

	var canvasImg = [];
	var savaImg;
	var requestCode = '55555abc';
	

	var conserveProgramme = document.getElementById('conserveProgramme');
	var styleSave = document.getElementById('styleSave');
	var styleSaveContainer = document.getElementById('styleSaveContainer');
	conserveProgramme.onclick = function() { //保存方案
		styleSaveContainer.classList.remove('pageOut');
		styleSaveContainer.classList.add('pageIn');
	}
	styleSave.onclick = function() { //点击弹窗框的保存
		styleSaveContainer.classList.remove('pageIn');
		styleSaveContainer.classList.add('pageOut');
		console.log(savaImg);
	}


	function changeDiyPage() {
		var diyStyleDetailed = document.querySelectorAll('.diyStyleDetailed');
		var diyStyleBottomContainer = document.getElementById('diyStyleBottomContainer');
		var pageNum = -1;
		var currentPage = 0;
		diyStyleBottomContainer.innerHTML = ''; //先把切换页数的按钮组件清空
		for (var j = 0; j < Math.ceil(diyStyleDetailed.length / 10) + 2; j++) {
			pageNum++;
			var changeDiyStylePage = document.createElement('div');
			diyStyleBottomContainer.appendChild(changeDiyStylePage);
			changeDiyStylePage.setAttribute('class', 'changeDiyStylePage');
			changeDiyStylePage.innerHTML = pageNum;
		}
		document.querySelectorAll('.changeDiyStylePage')[0].innerHTML = '上一页';
		document.querySelectorAll('.changeDiyStylePage')[Math.ceil(diyStyleDetailed.length / 10) + 2 - 1].innerHTML = '下一页';
		if (Math.ceil(diyStyleDetailed.length / 10) < 4) {
			diyStyleBottomContainer.style.paddingLeft = (256 - 32 * (Math.ceil(diyStyleDetailed.length / 10)) - 58 * 2) / 2 + 'px';
		}
		var changeDiyStylePage = document.querySelectorAll('.changeDiyStylePage');
		changeDiyStylePage[1].setAttribute('current_page', '');
		var diyStyleDetailedContainerIn = document.getElementById('diyStyleDetailedContainerIn');
		for (var x = 1; x < changeDiyStylePage.length - 1; x++) {
			(function(x) {
				changeDiyStylePage[x].onclick = function() { //点击页数
					currentPage = x - 1;
					diyStyleDetailedContainerIn.style.top = -448 * (x - 1) + 'px';
					for (var ii = 0; ii < changeDiyStylePage.length - 2; ii++) {
						changeDiyStylePage[ii + 1].removeAttribute('current_page');
						changeDiyStylePage[x].setAttribute('current_page', '');
					}
				}
			})(x)
		}
		changeDiyStylePage[changeDiyStylePage.length - 1].onclick = function() { //点击下一页
			currentPage++;
			if (currentPage > changeDiyStylePage.length - 3) {
				currentPage = changeDiyStylePage.length - 3;
			}
			diyStyleDetailedContainerIn.style.top = -448 * (currentPage) + 'px';
			if (diyStyleDetailedContainerIn.offsetTop < -448 * (changeDiyStylePage.length - 2)) {
				diyStyleDetailedContainerIn.style.top = -448 * (changeDiyStylePage.length - 2) + 'px';
			}
			for (var ii = 0; ii < changeDiyStylePage.length - 2; ii++) {
				changeDiyStylePage[ii + 1].removeAttribute('current_page');
				changeDiyStylePage[currentPage + 1].setAttribute('current_page', '');
			}
		}
		changeDiyStylePage[0].onclick = function() { //点击上一页
			currentPage--;
			if (currentPage < 0) {
				currentPage = 0;
			}
			diyStyleDetailedContainerIn.style.top = -448 * (currentPage) + 'px';
			if (diyStyleDetailedContainerIn.offsetTop > 0) {
				diyStyleDetailedContainerIn.style.top = 0 + 'px';
			}
			for (var ii = 0; ii < changeDiyStylePage.length - 2; ii++) {
				changeDiyStylePage[ii + 1].removeAttribute('current_page');
				changeDiyStylePage[currentPage + 1].setAttribute('current_page', '');
			}
		}
		var diyStyleDetailed = document.querySelectorAll('.diyStyleDetailed');
		for (var i = 0; i < diyStyleDetailed.length; i++) {
			(function(i) {
				diyStyleDetailed[i].onclick = function() {
					for (var ii = 0; ii < diyStyleDetailed.length; ii++) {
						diyStyleDetailed[ii].removeAttribute('diyStyleChoose');
					}
					diyStyleDetailed[i].setAttribute('diyStyleChoose', '');
				}
			})(i)
		}
	}


}

function menuItem3() {
	var menuItem = document.querySelectorAll('.menuItem');
	var partContainer = document.querySelectorAll('.partContainer');
	// var goodsNum = 1;
	var currentPart3_item = 1;
	menuItem[2].onclick = function() {
		for (var i = 0; i < partContainer.length; i++) {
			partContainer[i].classList.remove('pageIn');
			partContainer[i].classList.add('pageOut');
			menuItem[i].classList.remove('choosed');
		}
		menuItem[2].classList.add('choosed');
		partContainer[2].classList.remove('pageOut');
		partContainer[2].classList.add('pageIn');
	}
	var part3_next = document.querySelector('.part3_next'); //下一步

	//功能定位下面五个栏目的点击
	var part3_item = document.getElementById('part3_contentContainer1').querySelectorAll('.part3_item');
	var part3_partContainer = partContainer[2].querySelectorAll('.part3_partContainer');
	for (var j = 0; j < part3_item.length; j++) {
		(function(j) {
			part3_item[j].onclick = function() {
				currentPart3_item = j + 1;
				if (currentPart3_item == 5) {
					part3_next.innerHTML = '提交';
				} else {
					part3_next.innerHTML = '下一步';
				}
				for (var i = 0; i < part3_partContainer.length; i++) {
					part3_partContainer[i].classList.remove('pageIn');
					part3_partContainer[i].classList.add('pageOut');
					part3_item[i].removeAttribute('part3_current');
				}
				part3_item[j].setAttribute('part3_current', '');
				part3_partContainer[j].classList.remove('pageOut');
				part3_partContainer[j].classList.add('pageIn');
			}
		})(j)
	}

	var goodsAdd = document.getElementById('goodsAdd');
	goodsAdd.onclick = function() {
		var goodsAllContainer = document.getElementById('goodsAllContainer');
		var goodsContainer = document.createElement('div');
		goodsContainer.classList.add('goodsContainer');
		goodsAllContainer.appendChild(goodsContainer);
		goodsContainer.innerHTML =
			'<div>' + '物品名称' + "<input class='questionInput'>" + '</div>' + '<div>' + '尺寸规格' + '<input class="questionInput">' + '</div>' + '<div>' + '使用位置' + '<input class="questionInput" style="width:180px;">' + '</div>' + '<div class="goodsDelete">' + '删除' + '</div>';
		goodsDeleteClick();
		// goodsNum++;
	}
	goodsDeleteClick();

	function goodsDeleteClick() {
		var goodsDelete = document.querySelectorAll('.goodsDelete');
		var goodsContainer = document.querySelectorAll('.goodsContainer');
		for (var i = 0; i < goodsDelete.length; i++) {
			(function(i) {
				goodsDelete[i].onclick = function() {
					// if (goodsNum > 1) {
					goodsAllContainer.removeChild(goodsContainer[i]);
					// goodsNum--;
					// }
				}
			})(i)
		}
	}
	var part3_more = document.querySelectorAll('.part3_more');
	var part3_contentContainer2_more = document.getElementById('part3_contentContainer2_more');
	var closePart3More = document.getElementById('closePart3More');
	var part3more_left = document.querySelector('.part3more_left');
	var part3_moreText = document.querySelector('.part3_moreText');
	var part3more_title = document.getElementById('part3more_title');
	for (var i = 0; i < part3_more.length; i++) {
		(function(i) {
			part3_more[i].onclick = function() {
				if (i == 0) {
					part3more_left.style.backgroundImage = 'url(src/media/equipment.png)';
					part3_moreText.innerHTML = '&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp在装修时可以选择壁挂式空调或中央空调，现今几乎所有的装修客户都会选择中央空调，家用中央空调的类型为分三种：水系统、风系统和冷媒系统，您可以根据自己住宅的特点，选配不同类型的家用中央空调。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp中央空调的优点：舒适度高： 家庭用中央空调每台室内机分别有一个送风口和一个回风口，气流循环更合理，室内温度更均匀，能够坚持±1℃的恒温状况，给人非常好的感受，尤其是水体系中央空调，舒适性十分高；而传统家用壁挂机和柜机，非常容易呈现气流死角，室内温差显着，容易得空调病。节约空间，显示装饰层次：家庭用中央空调室内机荫蔽在吊顶内，避免了像柜机相同安放占用室内空间，给家私的摆放带来更多自在，如在安放柜机的当地放个花架等；别的，以“藏”为美的装置方法能够和装饰装饰极好的交融，无形之中晋升房间装饰作用和层次，而一般柜机和壁挂机的管道线路露出在外，很不美观。使用成本低：传统分体机受室内环境温度改变影响较大，定频空调压缩机启动频繁，耗电大，恒温效果不明显。而中央空调选用直流变频技术和新冷媒技术，每台室内机可独自操控，可以有针对性的对房间进行温度调节，使用方便，耗电小。而且，房间面积越大，装置中央空调越合算。使用寿命长：家用分体空调（壁挂机和柜机）的使用寿命为5-8年之间，而家庭用中央空调的使用寿命在15-20年之间，其间，特灵中央空调水体系运用寿命为25年，如果是地源热泵中央空调体系，体系运用寿命为50年。所以，中央空调的运用寿命至少是家用机的一倍。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp别墅的中央空调系统不仅仅承担调节温度的责任，还需要考虑室内空气的换新，对于居住健康十分关键。尤其是地下室的空间，空气很难直接对流换新，有必要加上新风的功能，这样地下室不易潮湿，在里面会更加舒适。在选择最终的配置方案时，节能也是需要重点考虑的，功率的选择并不是全部越大越好，根据不同的空间选择合适的就可以。'
					part3more_title.innerHTML = '中央空调系统';
				}
				if (i == 1) {
					part3more_left.style.backgroundImage = 'url(src/media/equipment2.png)';
					part3_moreText.innerHTML = '&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp中央水处理系统包括中央净水、中央软水、中央纯水、中央热水四个部分。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp中央净水系统先有效清除水中的氯、重金属、细菌、病毒、藻类及固体悬浮物，后用活性炭进一步去除各有机特，让出水清澈、洁净、无卤，可直接饮用；系统具备自动维护功能。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp中央软水系统是通过天然树脂置换出水中的钙、镁离子等，降低水的硬度；有效减少对衣物的磨损，保护人体皮肤，避免管道、洁具、卫浴设备等结垢问题。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp中央纯水系统采用反渗透法，经精密计算的五道过滤程序，使出水变为纯净水，不含任何杂质和矿物质。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp中央热水系统是指用一台热水器随时随地多处同时供应热水。热水集中在一个地方产生，大容量的热水同时，多头通过保温管道供给热水。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp专家建议，为了您的自身健康和设备的安全，十分有必要安装中央水处理系统。'
					part3more_title.innerHTML = '中央水处理系统';
				}
				if (i == 2) {
					part3more_left.style.backgroundImage = 'url(src/media/equipment3.png)';
					part3_moreText.innerHTML = '&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp家庭影间系统也称家庭影院，这是由影音源(VCD、DVD等)、电视机、功率(AV)放大器和音响等组成的家庭视听系统。家庭影音系统的组成十分灵活，可按自己不同的要求来配置，有的偏重于卡拉OK功能，有的偏重于高保真的音乐欣赏，有的偏重于大屏幕前的“影院”环境音效。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp客户一般选择在地下室安装此系统，属于地下室娱乐系统的一个重要功能组合，在组织家庭Party时使用率最高，平时晚上也可以和家人一起看会高清大片，唱唱卡拉OK。';
					part3more_title.innerHTML = '家庭影音系统';
				}
				if (i == 3) {
					part3more_left.style.backgroundImage = 'url(src/media/equipment4.png)';
					part3_moreText.innerHTML = '&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp在一定的地域运用工程技术的艺术手段，通过改造地形(或进一步筑山、叠石、理水)种植树木花草，营造建筑和布置园路等途径创作而成的美的自然环境和游憩境遇，就称为园林。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp别墅园林设计除了庭院的设计，您还可以选择做屋顶花园，底楼花园，室内景观等。园林设计也有风格之分，近两年软流行的是美式、现代和中式。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp别墅庭院设计的好坏，需从科学性、文化性、实用性和艺术性等多方面来衡量。有人把宅院设计和园林景观设计混为一谈，其实二者不尽相同，园林是为了观赏，而庭院是为了生活。庭院更多要融入主人的个性和生活情趣，把艺术生活化。';
					part3more_title.innerHTML = '园林系统';
				}
				if (i == 4) {
					part3more_left.style.backgroundImage = 'url(src/media/equipment5.png)';
					part3_moreText.innerHTML = '&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp个性化品质生活已经日益引领我们现代生活的潮流，逐步的改变着我们的生活方向及方式，大大的提升了我们的生活品质。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp个性化生活系统包括：桑拿、高温瑜珈、酒窖、室内外泳池、室内电梯、室内保龄球、室内高尔夫等。';
					part3more_title.innerHTML = '个性化品质生活系统';
				}
				if (i == 5) {
					part3more_left.style.backgroundImage = 'url(src/media/equipment6.png)';
					part3_moreText.innerHTML = '&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp将科技带进生活，您会发现生活有了更多的便利和幸福。智能家居系统的核心设计理念：以人为本，充分考虑人的需要。我们提倡适度的应有智能家居技术，以便捷实用作为衡量标准。在选择智能家居体系统时，要注重功能定制时和供应商的沟通，设计出真正适合您家的独特的智能家居配置方案。<br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp智能家居系统几乎可以在家中的每一个角落配置，比如大门的指纹锁、可视对讲机、夜视防水摄像机、玄关和卫生间的感就灯、厨房和书房的背景音乐面板、自动窗帘、室内外的安防等；只要你想，你的家可以变得更智能。';
					part3more_title.innerHTML = '智能家居系统';
				}
				if (i == 6) {
					part3more_left.style.backgroundImage = '';
					part3_moreText.innerHTML = '';
					part3more_title.innerHTML = '照明设计系统';
				}
				part3_contentContainer2_more.classList.add('pageIn');
				part3_contentContainer2_more.classList.remove('pageOut');
				closePart3More.onclick = function() {
					part3_contentContainer2_more.classList.add('pageOut');
					part3_contentContainer2_more.classList.remove('pageIn');
				}
			}
		})(i)
	}
	var part3_container5_top = document.querySelectorAll('.part3_container5_top');
	var part3_contentContainer5 = document.querySelectorAll('.part3_contentContainer5');
	for (var i = 0; i < part3_container5_top.length; i++) {
		(function(i) {
			part3_container5_top[i].onclick = function() {
				for (var j = 0; j < part3_container5_top.length; j++) {
					part3_container5_top[j].removeAttribute('spaceChoose');
					part3_contentContainer5[j].classList.remove('pageIn');
					part3_contentContainer5[j].classList.add('pageOut');
				}
				part3_container5_top[i].setAttribute('spaceChoose', '');
				part3_contentContainer5[i].classList.remove('pageOut');
				part3_contentContainer5[i].classList.add('pageIn');

			}
		})(i)
	}
	part3_next.onclick = function() {
		if (currentPart3_item < 5) {
			part3_next.innerHTML = '下一步';
			for (var i = 0; i < part3_partContainer.length; i++) {
				part3_partContainer[i].classList.remove('pageIn');
				part3_partContainer[i].classList.add('pageOut');
				part3_item[i].removeAttribute('part3_current');
			}
			part3_item[currentPart3_item].setAttribute('part3_current', '');
			part3_partContainer[currentPart3_item].classList.remove('pageOut');
			part3_partContainer[currentPart3_item].classList.add('pageIn');
		}
		currentPart3_item++;
		if (currentPart3_item == 5) {
			part3_next.innerHTML = '提交';
		}
		if (currentPart3_item >= 6) {
			var Usage = [];
			var Work = [];
			var Anniversary = [];
			var Family = [];
			var GoodsHad = [];
			var Equipment = [];
			var Intrest = [];
			var Material = [];
			var Space = [];
			for (var i = 0; i < document.getElementsByName('use').length; i++) {
				if (document.getElementsByName('use')[i].checked) {
					var jsonUsage = {};
					jsonUsage.Name = document.getElementsByName('use')[i].value;
					Usage.push(jsonUsage);
				}
			}
			for (var i = 0; i < document.getElementsByName('job').length; i++) {
				if (document.getElementsByName('job')[i].checked) {
					var jsonWork = {};
					jsonWork.Name = document.getElementsByName('job')[i].value;
					Work.push(jsonWork);
				}
			}
			for (var i = 0; i < document.getElementsByName('anniversary').length; i++) {
				if (document.getElementsByName('anniversary')[i].checked) {
					var jsonAnniversary = {};
					jsonAnniversary.Name = document.getElementsByName('anniversary')[i].value;
					Anniversary.push(jsonAnniversary);
				}
			}
			for (var i = 0; i < document.getElementsByName('family').length; i++) {
				if (document.getElementsByName('family')[i].checked) {
					var jsonFamily = {};
					jsonFamily.Name = document.getElementsByName('family')[i].value;
					jsonFamily.Age = document.querySelectorAll('.familyContainer')[i].querySelectorAll('.questionInput')[0].value;
					jsonFamily.Work = document.querySelectorAll('.familyContainer')[i].querySelectorAll('.questionInput')[1].value;
					jsonFamily.Hobby = document.querySelectorAll('.familyContainer')[i].querySelectorAll('.questionInput')[2].value;
					Family.push(jsonFamily);
				}
			}
			for (var i = 0; i < document.querySelectorAll('.goodsContainer').length; i++) {
				var jsonGoodsHad = {};
				jsonGoodsHad.Name = document.querySelectorAll('.goodsContainer')[i].querySelectorAll('.questionInput')[0].value;
				jsonGoodsHad.Size = document.querySelectorAll('.goodsContainer')[i].querySelectorAll('.questionInput')[1].value;
				jsonGoodsHad.Position = document.querySelectorAll('.goodsContainer')[i].querySelectorAll('.questionInput')[2].value;
				GoodsHad.push(jsonGoodsHad);
			}
			for (var i = 0; i < document.getElementsByName('equipmentType').length; i++) { //设备系统
				if (document.getElementsByName('equipmentType')[i].checked) {
					var jsonEquipment = {};
					jsonEquipment.Name = document.getElementsByName('equipmentType')[i].value;
					Equipment.push(jsonEquipment);
				}
			}
			for (var i = 0; i < document.getElementsByName('interest').length; i++) { //兴趣展现
				if (document.getElementsByName('interest')[i].checked) {
					var jsonIntrest = {};
					jsonIntrest.Name = document.getElementsByName('interest')[i].value;
					Intrest.push(jsonIntrest);
				}
			}
			for (var i = 0; i < document.getElementsByName('material').length; i++) { //材料喜好
				if (document.getElementsByName('material')[i].checked) {
					var jsonMaterial = {};
					jsonMaterial.Name = document.getElementsByName('material')[i].value;
					Material.push(jsonMaterial);
				}
			}
			for (var i = 0; i < document.querySelectorAll('.spaceContainer').length; i++) { //空间分类
				(function(i) {
					var jsonSpace = {};
					jsonSpace.Name = document.querySelectorAll('.spaceContainer')[i].querySelector('.part3_title').innerHTML;

					var option = [];
					for (var j = 0; j < document.querySelectorAll('.spaceContainer')[i].querySelectorAll('.chooseInput').length; j++) {
						if (document.querySelectorAll('.spaceContainer')[i].querySelectorAll('.chooseInput')[j].checked) {
							var jsonOption = {};
							jsonOption.Name = document.querySelectorAll('.spaceContainer')[i].querySelectorAll('.chooseInput')[j].value;
							option.push(jsonOption);
						}
					}
					jsonSpace.Options = option;
					Space.push(jsonSpace);
				})(i)
			}
			var dataJson = {
				Name: document.getElementById('customerName').value,
				Age: document.getElementById('customerAge').value,
				Usage: Usage,
				Work: Work,
				Address: document.querySelector('.addressContainer').querySelectorAll('.questionInput')[0].value + '区/县' + document.querySelector('.addressContainer').querySelectorAll('.questionInput')[1].value + '街道' + document.querySelector('.addressContainer').querySelectorAll('.questionInput')[2].value + '小区' + document.querySelector('.addressContainer').querySelectorAll('.questionInput')[3].value + '单元' + document.querySelector('.addressContainer').querySelectorAll('.questionInput')[4].value + '室',
				Phone: document.getElementById('customerPhoneNumber').value,
				Anniversary: Anniversary,
				GoodsHad: GoodsHad,
				Family: Family,
				AgreedTime: document.getElementById('time').value,
				Equipment: Equipment,
				Intrest: Intrest,
				Material: Material,
				Space: Space
			}
			if (dataJson.Name == '' || dataJson.Age == '' || dataJson.Phone == '' || dataJson.Usage == '') {
				alert('还有未填写的内容！');
			} else {
				// console.log(JSON.stringify(dataJson))
				var xmlhttp = new XMLHttpRequest();
				xmlhttp.onreadystatechange = function() {
					if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {}
				}
				xmlhttp.open("POST", urlAddress, true);
				xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
				xmlhttp.send(JSON.stringify(dataJson));
				alert('提交成功！')
			}
		}
	}
}

function menuItem4() {
	var menuItem = document.querySelectorAll('.menuItem');
	var partContainer = document.querySelectorAll('.partContainer');
	var part4_contentContainer2 = document.querySelectorAll('.part4_contentContainer2');
	var part4_item = document.getElementById('part4_contentContainer1').querySelectorAll('.part3_item');
	menuItem[3].onclick = function() {
		for (var i = 0; i < partContainer.length; i++) {
			partContainer[i].classList.remove('pageIn');
			partContainer[i].classList.add('pageOut');
			menuItem[i].classList.remove('choosed');
		}
		menuItem[3].classList.add('choosed');
		partContainer[3].classList.remove('pageOut');
		partContainer[3].classList.add('pageIn');
	}
	for (var i = 0; i < part4_item.length; i++) {
		(function(i) {
			part4_item[i].onclick = function() {
				for (var j = 0; j < part4_item.length; j++) {
					part4_item[j].removeAttribute('part3_current');
					part4_contentContainer2[j].classList.remove('pageIn');
					part4_contentContainer2[j].classList.add('pageOut');
				}
				part4_item[i].setAttribute('part3_current', '');
				part4_contentContainer2[i].classList.add('pageIn');
				part4_contentContainer2[i].classList.remove('pageOut');
			}
		})(i)
	}
	for (var j = 0; j < part4_contentContainer2.length; j++) {
		(function(j) {
			var part4_vipContainer = part4_contentContainer2[j].querySelectorAll('.part4_vipContainer');
			var part4_vipBackground = part4_contentContainer2[j].querySelectorAll('.part4_vipBackground');
			var vipClose = part4_contentContainer2[j].querySelectorAll('.vipClose');
			var part4_Container = document.querySelectorAll('.part4_Container');
			for (var i = 0; i < vipClose.length; i++) {
				(function(i) {
					vipClose[i].onclick = function() {
						part4_contentContainer2[j].removeChild(part4_vipContainer[i]);
					}
					part4_vipBackground[i].onclick = function() { //点击vip查看详细信息
						part4_Container[0].classList.remove('pageIn');
						part4_Container[0].classList.add('pageOut');
						part4_Container[1].classList.add('pageIn');
						part4_Container[1].classList.remove('pageOut');
					}
				})(i)
			}

			var part4_detailedBack = document.getElementById('part4_detailedBack');
			part4_detailedBack.onclick = function() {
				part4_Container[1].classList.remove('pageIn');
				part4_Container[1].classList.add('pageOut');
				part4_Container[0].classList.add('pageIn');
				part4_Container[0].classList.remove('pageOut');
			}
		})(j)
	}

}