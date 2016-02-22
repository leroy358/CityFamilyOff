window.addEventListener("load", menuItem3);
window.addEventListener("load", xingqu);
window.addEventListener("load", spaceOther);
window.addEventListener("load", materialOther);
var moreShow = false;
var spaceOtherShow = [];;
var materialmoreShow = false;

function spaceOther() {
	for (var i = 0; i < document.querySelectorAll('.spaceContainer').length; i++) {
		(function(i) {
			spaceOtherShow[i] = false;
			document.querySelectorAll('.spaceContainer')[i].querySelector('.spaceClick').onclick = function() {
				if (spaceOtherShow[i]) {
					document.querySelectorAll('.spaceContainer')[i].querySelector('.spacemore').style.display = 'none';
					spaceOtherShow[i] = false;
				} else if (!spaceOtherShow[i]) {
					document.querySelectorAll('.spaceContainer')[i].querySelector('.spacemore').style.display = 'block';
					spaceOtherShow[i] = true;
				}
			}
		})(i)

	}
}

function materialOther() {
	var materialClick = document.querySelector('.materialClick');
	var materialmore = document.querySelector('.materialmore');
	materialClick.onclick = function() {
		if (!materialmoreShow) {
			materialmore.style.display = 'block';
			materialmoreShow=true;
		}else if(materialmoreShow){
			materialmore.style.display = 'none';
			materialmoreShow=false;
		}
	}
}

function xingqu() {
	var part3_typeChoose_XQ = document.querySelectorAll('.interestClick');
	var XQ_pic = document.getElementById('part3_contentContainer3').querySelector('.part3_contentContainer2_leftpic');
	var interestLeftpic_bottom = document.querySelector('.interestLeftpic_bottom');
	for (var i = 0; i < part3_typeChoose_XQ.length; i++) {
		(function(i) {
			part3_typeChoose_XQ[i].onclick = function() {
				if (i == 0) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu1.jpg)';
					interestLeftpic_bottom.innerHTML = '音乐：隔音吸音，预留插座同时展示音乐元素喜好。';
				}
				if (i == 1) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu2.jpg)';
					interestLeftpic_bottom.innerHTML = '舞蹈：舞蹈用品收藏，个人成就展示及影音隔音吸音。';
				}
				if (i == 2) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu3.jpg)';
					interestLeftpic_bottom.innerHTML = '品酒：储酒空间打造（包括防潮通风），储酒家具的选择。';
				}
				if (i == 3) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu4.png)';
					interestLeftpic_bottom.innerHTML = '茶道：茶具存储条件营造，接入水和漏水的处理。';
				}
				if (i == 4) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu5.jpg)';
					interestLeftpic_bottom.innerHTML = '旅行：旅行藏品、照片的展示，户外用品的存储。';
				}
				if (i == 5) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu6.png)';
					interestLeftpic_bottom.innerHTML = '宗教信仰：宗教用品展示，用具存储，预留足够电源。';
				}
				if (i == 6) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu7.jpg)';
					interestLeftpic_bottom.innerHTML = '收藏：展示空间的利用，特殊藏品的存储处理。';
				}
				if (i == 7) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu8.png)';
					interestLeftpic_bottom.innerHTML = '健身：健身用品的隔音和空间储藏，电子产品充电等。';
				}
				if (i == 8) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu9.png)';
					interestLeftpic_bottom.innerHTML = '养花：阳光房承重问题解决，花品种选择，漏水等处理。';
				}
				if (i == 9) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu10.jpg)';
					interestLeftpic_bottom.innerHTML = '书法绘画：氛围的营造、工具存放及案台空间设计。';
				}
				if (i == 10) {
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu11.png)';
					interestLeftpic_bottom.innerHTML = '宠物：宠物生活空间搭建及护理用品存放，卫生处理等。';
				}
				if (i == 11) {
					if (moreShow) {
						document.querySelector('.interestmore').style.display = 'none';
						moreShow = false;
					} else if (!moreShow) {
						document.querySelector('.interestmore').style.display = 'block';
						moreShow = true;
					}
					XQ_pic.style.backgroundImage = 'url(/src/media/xingqu12.jpg)';
					interestLeftpic_bottom.innerHTML = '其他：如有其它需求，请对接设计师做备注处理。';
				}
			}
		})(i)
	}
}

function menuItem3() {
	document.body.style.backgroundImage = 'url(/src/media/back.jpg)';
	var menuItem = document.querySelectorAll('.menuItem');
	var partContainer = document.querySelector('.partContainer');
	// var goodsNum = 1;
	var currentPart3_item = 1;

	var part3_next = document.querySelector('.part3_next'); //下一步

	//功能定位下面五个栏目的点击
	var part3_item = document.getElementById('part3_contentContainer1').querySelectorAll('.part3_item');
	var part3_partContainer = partContainer.querySelectorAll('.part3_partContainer');
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
	var familyAdd = document.getElementById('familyAdd');
	familyAdd.onclick = function() {
		var familysAllContainer = document.getElementById('familysAllContainer');
		var familysContainer = document.createElement('div');
		familysContainer.classList.add('familysContainer');
		familysAllContainer.appendChild(familysContainer);
		familysContainer.innerHTML =
			'<div>' + '关系' + "<input class='questionInput'>" + '</div>' + '<div>' + '年龄' + '<input class="questionInput">' + '</div>' + '<div>' + '职业' + "<input class='questionInput'>" + '</div>' + '<div>' + '兴趣爱好' + '<input class="questionInput" style="width:180px;">' + '</div>' + '<div>' + '生日' + '<input class="questionInput" >' + '</div>' + '<div class="familysDelete">' + '删除' + '</div>';
		familyDeleteClick();
		// goodsNum++;
	}
	familyDeleteClick();

	function familyDeleteClick() {
		var familysDelete = document.querySelectorAll('.familysDelete');
		var familysContainer = document.querySelectorAll('.familysContainer');
		for (var i = 0; i < familysDelete.length; i++) {
			(function(i) {
				familysDelete[i].onclick = function() {
					familysAllContainer.removeChild(familysContainer[i]);
				}
			})(i)
		}
	}
	var equipmentClick = document.querySelectorAll('.equipmentClick');
	var equipment_leftpic = document.querySelectorAll('.equipment_leftpic');
	var part3_contentContainer2_more = document.getElementById('part3_contentContainer2_more');
	var closePart3More = document.getElementById('closePart3More');
	var part3more_left = document.querySelector('.part3more_left');
	var part3_moreText = document.querySelector('.part3_moreText');
	var part3more_title = document.getElementById('part3more_title');
	var video = document.getElementById('video');
	var equipmentImages = document.querySelectorAll('.equipmentImages');
	var bannerdotE = document.querySelectorAll('.bannerdotE');
	equipment_leftpic[1].classList.remove('pageOut');
	equipment_leftpic[1].classList.add('pageIn');
	video.src = '/src/media/equipment1.mp4';
	var currentNumber = 0;
	var timer1;

	function move() {
		equipmentImages[currentNumber].removeAttribute('current');
		bannerdotE[currentNumber].removeAttribute('current', '');
		currentNumber++;
		if (currentNumber >= equipmentImages.length) {
			currentNumber = 0;
		}
		equipmentImages[currentNumber].setAttribute('current', '');
		bannerdotE[currentNumber].setAttribute('current', '');
	}
	for (var i = 0; i < equipmentImages.length; i++) {
		bannerdotE[i].index = i;
	}
	for (var i = 0; i < equipmentImages.length; i++) {
		bannerdotE[i].index = i;
		bannerdotE[i].onclick = function() {
			clearInterval(timer1);
			currentNumber = this.index;
			for (var j = 0; j < equipmentImages.length; j++) {
				equipmentImages[j].removeAttribute('current');
				bannerdotE[j].removeAttribute('current');
			}
			equipmentImages[currentNumber].setAttribute('current', '');
			bannerdotE[currentNumber].setAttribute('current', '');
			timer1 = setInterval(move, 2000);
		}
	}
	for (var i = 0; i < equipmentClick.length; i++) {
		(function(i) {
			equipmentClick[i].onclick = function() {
				if (i == 0) {
					timer1 = setInterval(move, 2000);
					equipment_leftpic[0].classList.remove('pageOut');
					equipment_leftpic[0].classList.add('pageIn');
					equipment_leftpic[1].classList.remove('pageIn');
					equipment_leftpic[1].classList.add('pageOut');
				}
				if (i == 1) {
					clearInterval(timer1);
					equipment_leftpic[0].classList.remove('pageIn');
					equipment_leftpic[0].classList.add('pageOut');
					equipment_leftpic[1].classList.remove('pageOut');
					equipment_leftpic[1].classList.add('pageIn');
					video.src = '/src/media/equipment1.mp4';
				}
				if (i == 2) {
					clearInterval(timer1);
					equipment_leftpic[0].classList.remove('pageIn');
					equipment_leftpic[0].classList.add('pageOut');
					equipment_leftpic[1].classList.remove('pageOut');
					equipment_leftpic[1].classList.add('pageIn');
					video.src = '/src/media/equipment2.mp4';
				}
				if (i == 3) {
					clearInterval(timer1);
					equipment_leftpic[0].classList.remove('pageIn');
					equipment_leftpic[0].classList.add('pageOut');
					equipment_leftpic[1].classList.remove('pageOut');
					equipment_leftpic[1].classList.add('pageIn');
					video.src = '/src/media/equipment2.mp4';
				}
				if (i == 4) {
					clearInterval(timer1);
					equipment_leftpic[0].classList.remove('pageIn');
					equipment_leftpic[0].classList.add('pageOut');
					equipment_leftpic[1].classList.remove('pageOut');
					equipment_leftpic[1].classList.add('pageIn');
					video.src = '/src/media/equipment3.mp4';
				}
				if (i == 5) {
					clearInterval(timer1);
					equipment_leftpic[0].classList.remove('pageIn');
					equipment_leftpic[0].classList.add('pageOut');
					equipment_leftpic[1].classList.remove('pageOut');
					equipment_leftpic[1].classList.add('pageIn');
					video.src = '/src/media/equipment5.mp4';
				}
				if (i == 6) {
				    clearInterval(timer1);
				    equipment_leftpic[0].classList.remove('pageIn');
				    equipment_leftpic[0].classList.add('pageOut');
				    equipment_leftpic[1].classList.remove('pageOut');
				    equipment_leftpic[1].classList.add('pageIn');
				    video.src = '/src/media/equipment6.mp4';
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
					part3_container5_top[j].classList.remove('spaceChoose');
					part3_contentContainer5[j].classList.remove('pageIn');
					part3_contentContainer5[j].classList.add('pageOut');
				}
				part3_container5_top[i].classList.add('spaceChoose');
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
			var MaterialNo = [];
			var Space = [];
			var MaterialOther='';
			for (var i = 0; i < document.getElementsByName('use').length; i++) {
				if (document.getElementsByName('use')[i].checked) {
					var jsonUsage = {};
					jsonUsage.Name = document.getElementsByName('use')[i].value;
					Usage.push(jsonUsage);
				}
			}
			for (var i = 0; i < document.getElementsByName('job').length; i++) {
				if (i != 15) {
					if (document.getElementsByName('job')[i].checked) {
						var jsonWork = {};
						jsonWork.Name = document.getElementsByName('job')[i].value;
						Work.push(jsonWork);
					}
				}
				if (i == 15) {
					if (document.getElementsByName('job')[i].checked & document.querySelectorAll('.jobmore')[0].querySelectorAll('.questionInput')[0].value != '') {
						var jsonWork = {};
						jsonWork.Other = document.querySelectorAll('.jobmore')[0].querySelectorAll('.questionInput')[0].value;
						Work.push(jsonWork);
					}
				}

			}
			// for (var i = 0; i < document.getElementsByName('anniversary').length; i++) {
			// 	if (document.getElementsByName('anniversary')[i].checked) {
			// 		var jsonAnniversary = {};
			// 		jsonAnniversary.Name = document.getElementsByName('anniversary')[i].value;
			// 		jsonAnniversary.Date = document.querySelectorAll('.specialContainer')[i].querySelectorAll('.questionInput')[0].value + '年 - ' + document.querySelectorAll('.specialContainer')[i].querySelectorAll('.questionInput')[1].value + '月 - ' + document.querySelectorAll('.specialContainer')[i].querySelectorAll('.questionInput')[2].value + '日';
			// 		Anniversary.push(jsonAnniversary);
			// 	}
			// }
			for (var i = 0; i < document.querySelectorAll('.familysContainer').length; i++) {
				var jsonFamily = {};
				jsonFamily.Name = document.querySelectorAll('.familysContainer')[i].querySelectorAll('.questionInput')[0].value;
				jsonFamily.Age = document.querySelectorAll('.familysContainer')[i].querySelectorAll('.questionInput')[1].value;
				jsonFamily.Work = document.querySelectorAll('.familysContainer')[i].querySelectorAll('.questionInput')[2].value;
				jsonFamily.Hobby = document.querySelectorAll('.familysContainer')[i].querySelectorAll('.questionInput')[3].value;
				jsonFamily.Birthday = document.querySelectorAll('.familysContainer')[i].querySelectorAll('.questionInput')[4].value;
				Family.push(jsonFamily);
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
			if (moreShow & document.querySelectorAll('.interestmore')[0].querySelectorAll('.questionInput')[0].value != '') {
				var jsonIntrest = {};
				jsonIntrest.Other = document.querySelectorAll('.interestmore')[0].querySelectorAll('.questionInput')[0].value;
				Intrest.push(jsonIntrest);
			}
			for (var i = 0; i < document.getElementsByName('material').length; i++) { //材料喜好
				if (document.getElementsByName('material')[i].checked) {
					var jsonMaterial = {};
					jsonMaterial.Name = document.getElementsByName('material')[i].value;
					Material.push(jsonMaterial);
				}
			}
			for (var i = 0; i < document.getElementsByName('material_no').length; i++) { //材料喜好
				if (document.getElementsByName('material_no')[i].checked) {
					var jsonMaterialNo = {};
					jsonMaterialNo.Name = document.getElementsByName('material_no')[i].value;
					MaterialNo.push(jsonMaterialNo);
				}
			}
			for (var i = 0; i < document.querySelectorAll('.spaceContainer').length; i++) { //空间分类
				(function(i) {
					var jsonSpace = {};
					jsonSpace.Name = document.querySelectorAll('.spaceContainer')[i].querySelector('.part3_title').innerHTML;
					if (spaceOtherShow[i]) {
						jsonSpace.Other = document.querySelectorAll('.spaceContainer')[i].querySelectorAll('.questionInput')[0].value;
					} else {
						jsonSpace.Other = '';
					}
					Space.push(jsonSpace);
				})(i)
			}
			if(materialmoreShow){
				MaterialOther=document.querySelector('.materialmore').querySelector('.questionInput').value;
			}else{
				MaterialOther='';
			}
			var dataJson = {
				Name: document.getElementById('customerName').value,
				Age: '',
				Usage: Usage,
				Work: Work,
				Address: document.querySelector('.addressContainer').querySelectorAll('.questionInput')[0].value + '区/县 - ' + document.querySelector('.addressContainer').querySelectorAll('.questionInput')[1].value + '街道 - ' + document.querySelector('.addressContainer').querySelectorAll('.questionInput')[2].value + '小区 - ' + document.querySelector('.addressContainer').querySelectorAll('.questionInput')[3].value + '单元 - ' + document.querySelector('.addressContainer').querySelectorAll('.questionInput')[4].value + '室',
				Phone: document.getElementById('customerPhoneNumber').value,
				Xiaoqu: document.getElementById('customerXiaoQu').value,
				Anniversary: Anniversary,
				GoodsHad: GoodsHad,
				Family: Family,
				AgreedTime: document.getElementById('time').value,
				Equipment: Equipment,
				Intrest: Intrest,
				Material: Material,
				MaterialNo: MaterialNo,
				MaterialOther: MaterialOther,
				FurnitureViewUrl: document.getElementById('FurnitureViewUrl').value,
				diyResultUrl:document.getElementById('diyResultUrl').value,
				Space: Space
			}
			if (dataJson.Name == '' || dataJson.Phone == '' || dataJson.Usage == '') {
				alert('还有未填写的内容！');
			} else {
				// console.log(JSON.stringify(dataJson))
				var xmlhttp = new XMLHttpRequest();
				xmlhttp.onreadystatechange = function() {
					if (xmlhttp.readyState == 4) {
						if (xmlhttp.status == 200) {
						    alert('提交成功！');
						    //alert(xmlhttp.responseText);
							location.reload();
						} else {
							alert('连接服务器失败，请稍后再试！');
						}
					}
				}
				xmlhttp.open("POST", urlAddress, true);
				xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
				//xmlhttp.send("dataJson=" + JSON.stringify(dataJson) + "&guestName=" + dataJson.Name + "&guestPhone=" + dataJson.Phone);
				xmlhttp.send(JSON.stringify(dataJson));
			}
		}
	}
}