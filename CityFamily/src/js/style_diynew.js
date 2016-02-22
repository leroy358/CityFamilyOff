window.addEventListener("load", menuItem2);

function menuItem2() {
	var savaImg;
	var imgurl = "http://119.188.113.104:8080";
	var yImgLast = [];
	var yImgHas = [];
	var yImg2 = [];
	var oC = document.getElementById('allcanvas');
	var oGC = oC.getContext('2d');
	var oGC2;
	ajaxDiy(requestCode);
	// var diyFull = document.getElementById('diyFull');
	// diyFull.onclick = function() {
	// 	launchFullscreen(document.getElementById("part2_contentContainer4_content")); // 启动全屏!
	// }

	// function launchFullscreen(element) {
	// 	if (element.requestFullscreen) {
	// 		element.requestFullscreen();
	// 	} else if (element.mozRequestFullScreen) {
	// 		element.mozRequestFullScreen();
	// 	} else if (element.webkitRequestFullscreen) {
	// 		element.webkitRequestFullscreen();
	// 	} else if (element.msRequestFullscreen) {
	// 		element.msRequestFullscreen();
	// 	}
	// }
	var allWidth = document.documentElement.clientWidth;
	window.onresize = function() {
		allWidth = document.documentElement.clientWidth;
	}

	function ajaxDiy(requestCode) {
		var xmlhttp1 = new XMLHttpRequest();
		var testJson = {
			"request": "getSampleRoom",
			"sampleRoom": {
				"requestCode": requestCode
			}
		};
		var jsonResult;
		xmlhttp1.onreadystatechange = function() {
			if (xmlhttp1.readyState == 4 && xmlhttp1.status == 200) {
				jsonResult = JSON.parse(xmlhttp1.responseText);
				diy(jsonResult);
			}
		}
		xmlhttp1.open("POST", diyUrl, true);
		xmlhttp1.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
		xmlhttp1.send(JSON.stringify(testJson));



		function diy(jsonResult) {
			var thingsindex = [];
			var allstyles = [];
			var allpicstyles = [];
			var perspectiveListIndex = 0;
			var operation = []; //操作存储位置
			var operateindex = 0; //操作占位符
			var hasLoadImg = [];
			if (getJson != '') {
				var sendJson = JSON.parse(getJson);
				perspectiveListIndex = sendJson.perspectiveListIndex;
				thingsindex = sendJson.thingsindex;
				allstyles = sendJson.allstyles;
				allpicstyles = sendJson.allpicstyles;
			}
			var diyThingsDetailedContainer = document.getElementById('diyThingsDetailedContainer');
			var diyThingsTitle = document.getElementById('diyThingsTitle');
			var diyLeft = document.getElementById('diyLeft');
			var diyRight = document.getElementById('diyRight');
			var diyBack = document.getElementById('diyBack');
			var diyAhead = document.getElementById('diyAhead');

			var sampleRoom = jsonResult.SampleRoom;
			var itemPlaceholderList = sampleRoom.itemPlaceholderList;
			var perspectiveList = sampleRoom.perspectiveList;
			var materialList = sampleRoom.materialList;
			var room = new Object();
			Currentroom();
			var canvasImg = [];
			diyLeft.onclick = function() {
				perspectiveListIndex = perspectiveListIndex - 1 < 0 ? perspectiveList.length - 1 : perspectiveListIndex - 1;
				//operation.splice(operateindex+1,operation.length-1);
				//var operateobject = new Object();
				//operateobject.thingsindex = thingsindex;
				//operateobject.allstyles = allstyles;
				//operateobject.allpicstyles = allpicstyles;
				//operateobject.perspectiveListIndex = perspectiveListIndex;
				//operation[operation.length]=JSON.stringify(operateobject);
				//operateindex=operation.length-1;
				//console.log(operation)
				Currentroom();
			}
			diyRight.onclick = function() {
				perspectiveListIndex = perspectiveListIndex + 1 > perspectiveList.length - 1 ? 0 : perspectiveListIndex + 1;
				//operation.splice(operateindex+1,operation.length-1);
				//var operateobject = new Object();
				//operateobject.thingsindex = thingsindex;
				//operateobject.allstyles = allstyles;
				//operateobject.allpicstyles = allpicstyles;
				//operateobject.perspectiveListIndex = perspectiveListIndex;
				//operation[operation.length]=JSON.stringify(operateobject);
				//operateindex=operation.length-1;
				//console.log(operation)
				Currentroom();
			}

			function backAhead() {
				diyBack.onclick = function() {
					operateindex = operateindex - 1 < 0 ? 0 : operateindex - 1;
					thingsindex = JSON.parse(operation[operateindex]).thingsindex;
					allstyles = JSON.parse(operation[operateindex]).allstyles;
					allpicstyles = JSON.parse(operation[operateindex]).allpicstyles;
					//perspectiveListIndex = JSON.parse(operation[operateindex]).perspectiveListIndex;
					// console.log(thingsindex)
					// console.log(operation)
					Currentroom();
				}
				diyAhead.onclick = function() {
					operateindex = operateindex + 1 > operation.length - 1 ? operation.length - 1 : operateindex + 1;
					thingsindex = JSON.parse(operation[operateindex]).thingsindex;
					allstyles = JSON.parse(operation[operateindex]).allstyles;
					allpicstyles = JSON.parse(operation[operateindex]).allpicstyles;
					//perspectiveListIndex = JSON.parse(operation[operateindex]).perspectiveListIndex;
					Currentroom();
				}
			}


			function Currentroom() {
				room = new Object();
				if (perspectiveList.length > perspectiveListIndex) {
					room.backgroundImageUrl = imgurl + perspectiveList[perspectiveListIndex].backgroundImageUrl;
					var visibleItemPlaceholderListArray = [];
					for (var i = 0; i < perspectiveList[perspectiveListIndex].visibleItemPlaceholderList.length; i++) {
						var visibleItemPlaceholderListItem = new Object();
						visibleItemPlaceholderListItem.typeId = perspectiveList[perspectiveListIndex].visibleItemPlaceholderList[i].id;
						visibleItemPlaceholderListItem.displayIndex = perspectiveList[perspectiveListIndex].visibleItemPlaceholderList[i].displayIndex;
						for (var y = 0; y < itemPlaceholderList.length; y++) {
							if (itemPlaceholderList[y].id == visibleItemPlaceholderListItem.typeId) {
								visibleItemPlaceholderListItem.typeName = itemPlaceholderList[y].name;
								var objectArray = [];
								for (var z = 0; z < itemPlaceholderList[y].replaceableItemList.length; z++) {
									var object = new Object();
									object.id = itemPlaceholderList[y].replaceableItemList[z].id;
									object.name = itemPlaceholderList[y].replaceableItemList[z].name;
									object.asDefaultItem = itemPlaceholderList[y].replaceableItemList[z].asDefaultItem;
									if (object.asDefaultItem) {
										var is = false;
										for (var j = 0; j < allpicstyles.length; j++) {
											if (allpicstyles[j].name == visibleItemPlaceholderListItem.typeName) {
												is = true;
												break;
											};
										};
										if (!is) {
											var picstyleitem = new Object();
											picstyleitem.name = visibleItemPlaceholderListItem.typeName;
											picstyleitem.selectName = object.name;
											picstyleitem.display = 'block';
											allpicstyles.push(picstyleitem);
										};
									};
									var keywords;
									if (itemPlaceholderList[y].replaceableItemList[z].styleKeywords != null) {
										keywords = itemPlaceholderList[y].replaceableItemList[z].styleKeywords;
									} else if (itemPlaceholderList[y].replaceableItemList[z].useDictionaryItem.styleKeywords != null) {
										keywords = itemPlaceholderList[y].replaceableItemList[z].useDictionaryItem.styleKeywords;
									} else {
										keywords = "";
									}
									object.styleKeywords = keywords.split("|");
									object.thumbnailUrl = imgurl + itemPlaceholderList[y].replaceableItemList[z].useDictionaryItem.thumbnailUrl;
									for (var h = 0; h < materialList.length; h++) {
										if (perspectiveList[perspectiveListIndex].id == materialList[h].forPerspective.id && object.id == materialList[h].forReplaceableItem.id) {
											object.imageUrl = imgurl + materialList[h].imageUrl;
											// object.display='block';
											break;
										};
									};
									objectArray.push(object);
								};
								var allStyleKeyWords = [];
								var currentAllKeywords = [];
								for (var z = 0; z < objectArray.length; z++) {
									allStyleKeyWords.push(objectArray[z].styleKeywords);
								}
								currentAllKeywords = ['全部风格'].concat(divideArray(allStyleKeyWords));
								visibleItemPlaceholderListItem.currentAllKeywords = currentAllKeywords;
								visibleItemPlaceholderListItem.objectArray = objectArray;
								break;
							};
						};
						visibleItemPlaceholderListArray.push(visibleItemPlaceholderListItem);
					};
					room.array = visibleItemPlaceholderListArray;
					if (operation.length == 0) {
						var operateobject = new Object();
						operateobject.thingsindex = thingsindex;
						operateobject.allstyles = allstyles;
						operateobject.allpicstyles = allpicstyles;
						operateobject.perspectiveListIndex = perspectiveListIndex;
						operation[operation.length] = JSON.stringify(operateobject);
						operateindex = operation.length - 1;
						// console.log(operation)
					}

					type(room);
					styles(room);
					picStyles(room);
					setImgs(room);
				};
			}

			function type(room) {
				diyThingsDetailedContainer.innerHTML = '';
				for (var i = 0; i < room.array.length; i++) {
					var diyThingOut = document.createElement('div');
					diyThingOut.setAttribute('class', 'diyThingOut');
					var diyThings = document.createElement('div');
					diyThingsDetailedContainer.appendChild(diyThingOut);
					diyThings.setAttribute('class', 'diyThings');
					diyThings.innerHTML = room.array[i].typeName;
					var thingsEye = document.createElement('span');
					thingsEye.setAttribute('class', 'thingsEye');
					for (var ii = 0; ii < allpicstyles.length; ii++) {
						if (room.array[i].typeName == allpicstyles[ii].name) {
							if (allpicstyles[ii].display == 'block') {
								thingsEye.setAttribute('visible', '');
							} else if (allpicstyles[ii].display == 'none') {
								thingsEye.setAttribute('invisible', '');
							}
						}
					}
					// thingsEye.setAttribute('visible', '');
					diyThingOut.appendChild(thingsEye);
					diyThingOut.appendChild(diyThings);
					if (thingsindex[perspectiveListIndex] == null) {
						thingsindex[perspectiveListIndex] = 0;
						var styleitem = new Object();
						styleitem.name = room.array[i].typeName;
						allstyles.push(styleitem);
					}
					diyThingsTitle.getElementsByTagName('span')[0].innerHTML = room.array[thingsindex[perspectiveListIndex]].typeName;
					var diyThingsDetailedContainerShow = false;
					diyThingsTitle.onclick = function() { //DIY点击物件出现下拉框列表
						document.getElementById('diyThingsContainer').focus();
						if (!diyThingsDetailedContainerShow) {
							document.getElementById('diyThingsDetailedContainer').classList.remove('pageOut');
							document.getElementById('diyThingsDetailedContainer').classList.add('pageIn');
							diyThingsDetailedContainerShow = true;
						} else {
							document.getElementById('diyThingsDetailedContainer').classList.remove('pageIn');
							document.getElementById('diyThingsDetailedContainer').classList.add('pageOut');
							diyThingsDetailedContainerShow = false;
						}
					}

					document.getElementById('diyThingsContainer').onblur = function() {
						if (diyThingsDetailedContainerShow) {
							setTimeout(function() {
								document.getElementById('diyThingsDetailedContainer').classList.remove('pageIn');
								document.getElementById('diyThingsDetailedContainer').classList.add('pageOut');
							}, 200)
							diyThingsDetailedContainerShow = false;
						}
					}
					var diyThings = document.querySelectorAll('.diyThings');
					var thingsEye = document.querySelectorAll('.thingsEye');
					var diyThingsTitleSpan = diyThingsTitle.getElementsByTagName('span')[0];
					for (var idiyThings = 0; idiyThings < diyThings.length; idiyThings++) {
						(function(idiyThings) {
							diyThings[idiyThings].onclick = function() { //物件下拉框点击物件
								document.getElementById('diyThingsDetailedContainer').classList.remove('pageIn');
								document.getElementById('diyThingsDetailedContainer').classList.add('pageOut');
								diyThingsDetailedContainerShow = false;
								diyThingsTitleSpan.innerHTML = diyThings[idiyThings].innerHTML;
								thingsindex[perspectiveListIndex] = idiyThings;
								var is = false;
								for (var j = 0; j < allstyles.length; j++) {
									if (allstyles[j].name == diyThings[idiyThings].innerHTML) {
										is = true;
										break;
									};
								}
								if (!is) {
									var styleitem = new Object();
									styleitem.name = diyThings[idiyThings].innerHTML;
									allstyles.push(styleitem);
									is = false;
								};
								for (var j = 0; j < allpicstyles.length; j++) {
									if (allpicstyles[j].name == diyThings[idiyThings].innerHTML) {
										is = true;
										break;
									};
								};
								if (!is) {
									var picstyleitem = new Object();
									picstyleitem.name = diyThings[idiyThings].innerHTML;
									allpicstyles.push(picstyleitem);
								};

								// operation.splice(operateindex + 1, operation.length - 1);
								// var operateobject = new Object();
								// operateobject.thingsindex = thingsindex;
								// operateobject.allstyles = allstyles;
								// operateobject.allpicstyles = allpicstyles;
								// operateobject.perspectiveListIndex = perspectiveListIndex;
								// operation[operation.length] = JSON.stringify(operateobject);
								// operateindex = operation.length - 1;
								// console.log(operation)

								styles(room);
								picStyles(room);
							}

							thingsEye[idiyThings].onclick = function() { //物件下拉框点击眼睛
								if (thingsEye[idiyThings].getAttribute('visible') != null) {
									thingsEye[idiyThings].removeAttribute('visible');
									thingsEye[idiyThings].setAttribute('invisible', '');
									for (var j = 0; j < allpicstyles.length; j++) {
										if (allpicstyles[j].name == diyThings[idiyThings].innerHTML) {
											allpicstyles[j].display = 'none';
										};
									};

								} else if (thingsEye[idiyThings].getAttribute('invisible') != null) {
									thingsEye[idiyThings].removeAttribute('invisible');
									thingsEye[idiyThings].setAttribute('visible', '');
									for (var j = 0; j < allpicstyles.length; j++) {
										if (allpicstyles[j].name == diyThings[idiyThings].innerHTML) {
											allpicstyles[j].display = 'block';
										};
									};
								}
								setImgs(room);
							}
						})(idiyThings)
					}
				}
				changeDiyPage();
			}

			function styles(room) {
				document.getElementById('diyStyleItemDetailedContainer').innerHTML = '';
				for (var y = 0; y < room.array[thingsindex[perspectiveListIndex]].currentAllKeywords.length; y++) {
					var diyStyle = document.createElement('div');
					document.getElementById('diyStyleItemDetailedContainer').appendChild(diyStyle);
					diyStyle.setAttribute('class', 'diyStyle');
					diyStyle.innerHTML = room.array[thingsindex[perspectiveListIndex]].currentAllKeywords[y];
				}
				var diyStylesDetailedContainerShow = false;
				document.getElementById('diyStyleTitle').onclick = function() { //DIY点击风格出现下拉框列表
					document.getElementById('diyStyleTopContainer').focus();
					if (!diyStylesDetailedContainerShow) {
						document.getElementById('diyStyleItemDetailedContainer').classList.remove('pageOut');
						document.getElementById('diyStyleItemDetailedContainer').classList.add('pageIn');
						diyStylesDetailedContainerShow = true;
					} else {
						document.getElementById('diyStyleItemDetailedContainer').classList.remove('pageIn');
						document.getElementById('diyStyleItemDetailedContainer').classList.add('pageOut');
						diyStylesDetailedContainerShow = false;
					}
				}
				document.getElementById('diyStyleTopContainer').onblur = function() {
					if (diyStylesDetailedContainerShow) {
						diyStylesDetailedContainerShow = false;
						setTimeout(function() {
							document.getElementById('diyStyleItemDetailedContainer').classList.remove('pageIn');
							document.getElementById('diyStyleItemDetailedContainer').classList.add('pageOut');
						}, 200)
					}
				}
				var diyStyle = document.querySelectorAll('.diyStyle');
				var diyStyleTitle = document.getElementById('diyStyleTitle');
				var diyStyleTitleSpan = diyStyleTitle.getElementsByTagName('span')[0];
				for (var i = 0; i < allstyles.length; i++) {
					if (allstyles[i].name == room.array[thingsindex[perspectiveListIndex]].typeName) {
						if (allstyles[i].index == null) {
							allstyles[i].index = 0;
						};
						diyStyleTitleSpan.innerHTML = room.array[thingsindex[perspectiveListIndex]].currentAllKeywords[allstyles[i].index];
						break;
					};
				};
				for (var idiyStyle = 0; idiyStyle < diyStyle.length; idiyStyle++) {
					(function(idiyStyle) {
						diyStyle[idiyStyle].onclick = function() {
							document.getElementById('diyStyleItemDetailedContainer').classList.remove('pageIn');
							document.getElementById('diyStyleItemDetailedContainer').classList.add('pageOut');
							diyStylesDetailedContainerShow = false;
							diyStyleTitleSpan.innerHTML = diyStyle[idiyStyle].innerHTML;
							for (var i = 0; i < allstyles.length; i++) {
								if (allstyles[i].name == room.array[thingsindex[perspectiveListIndex]].typeName) {
									allstyles[i].index = idiyStyle;
									break;
								};
							};

							// operation.splice(operateindex + 1, operation.length - 1);
							// var operateobject = new Object();
							// operateobject.thingsindex = thingsindex;
							// operateobject.allstyles = allstyles;
							// operateobject.allpicstyles = allpicstyles;
							// operateobject.perspectiveListIndex = perspectiveListIndex;
							// operation[operation.length] = JSON.stringify(operateobject);
							// operateindex = operation.length - 1;
							// console.log(operation)
							picStyles(room);
						}
					})(idiyStyle)
				}
				changeDiyPage();
			}

			function picStyles(room) {
				document.getElementById('diyStyleDetailedContainerIn').innerHTML = '';
				for (var i = 0; i < allstyles.length; i++) {
					if (allstyles[i].name == room.array[thingsindex[perspectiveListIndex]].typeName) {
						for (var y = 0; y < room.array[thingsindex[perspectiveListIndex]].objectArray.length; y++) {
							for (var j = 0; j < room.array[thingsindex[perspectiveListIndex]].objectArray[y].styleKeywords.length; j++) {
								if (room.array[thingsindex[perspectiveListIndex]].objectArray[y].styleKeywords[j] == room.array[thingsindex[perspectiveListIndex]].currentAllKeywords[allstyles[i].index] || allstyles[i].index == null || allstyles[i].index == 0) {
									var diyStyleDetailed = document.createElement('div');
									diyStyleDetailed.setAttribute('class', 'diyStyleDetailed');
									diyStyleDetailed.style.backgroundImage = 'url(' + room.array[thingsindex[perspectiveListIndex]].objectArray[y].thumbnailUrl + ')';
									var diyStyleDetailedBottom = document.createElement('div');
									diyStyleDetailedBottom.setAttribute('class', 'diyStyleDetailedBottom');
									diyStyleDetailedBottom.innerHTML = room.array[thingsindex[perspectiveListIndex]].objectArray[y].name;
									diyStyleDetailed.appendChild(diyStyleDetailedBottom);
									document.getElementById('diyStyleDetailedContainerIn').appendChild(diyStyleDetailed);
									break;
								};
							};
						}
						break;
					};
				};
				var diyStyleDetailed = document.querySelectorAll('.diyStyleDetailed');
				for (var z = 0; z < allpicstyles.length; z++) {
					if (room.array[thingsindex[perspectiveListIndex]].typeName == allpicstyles[z].name) {
						for (var i = 0; i < diyStyleDetailed.length; i++) {
							if (allpicstyles[z].selectName == diyStyleDetailed[i].querySelector('.diyStyleDetailedBottom').innerHTML) {
								diyStyleDetailed[i].classList.add('diyStyleChoose');
							};
						};
					};
				}
				for (var z = 0; z < diyStyleDetailed.length; z++) {
					(function(z) {
						diyStyleDetailed[z].onclick = function() {
							for (var q = 0; q < diyStyleDetailed.length; q++) {
								diyStyleDetailed[q].classList.remove('diyStyleChoose');
							}
							diyStyleDetailed[z].classList.add('diyStyleChoose');
							for (var i = 0; i < allpicstyles.length; i++) {
								if (allpicstyles[i].name == room.array[thingsindex[perspectiveListIndex]].typeName) {
									allpicstyles[i].selectName = diyStyleDetailed[z].querySelector('.diyStyleDetailedBottom').innerHTML;
									break;
								};
							};

							operation.splice(operateindex + 1, operation.length - 1);
							var operateobject = new Object();
							operateobject.thingsindex = thingsindex;
							operateobject.allstyles = allstyles;
							operateobject.allpicstyles = allpicstyles;
							operateobject.perspectiveListIndex = perspectiveListIndex;
							operation[operation.length] = JSON.stringify(operateobject);
							operateindex = operation.length - 1;
							backAhead();
							// console.log(operation)
							setImgs(room);
						}
					})(z)
				}
				changeDiyPage();
			}

			function divideArray(array) { //拆分数组里面的数组合并成一个数组
				var arrayreturn = [];
				for (var iarray = 0; iarray < array.length; iarray++) {
					(function(iarray) {
						if (array[iarray]) {
							var arraySingle = array[iarray];
							for (var iarraySingle = 0; iarraySingle < arraySingle.length; iarraySingle++) {
								if (arrayreturn.indexOf(arraySingle[iarraySingle]) == -1 & arraySingle[iarraySingle] != "") {
									arrayreturn.push(arraySingle[iarraySingle])
								}
							}
						}
					})(iarray)
				}
				return arrayreturn;
			}


			function setImgs(room) {
				var loading = document.getElementById('loading');
				loading.classList.remove("pageOut");
				loading.classList.add("pageIn");
				var c = [];
				for (var i = 0; i < yImgLast.length; i++) {
					if (yImgHas.indexOf(yImgLast[i]) < 0) {
						c.push(yImgLast[i]);
					}
				}
				for (var i = 0; i < yImgHas.length; i++) {
					if (yImgLast.indexOf(yImgHas[i]) < 0) {
						c.push(yImgHas[i]);
					}
				}
				for (var j = 0; j < c.length; j++) {
					for (var i = 0; i < yImg2.length; i++) {
						if (yImg2[i].src == c[j]) {
							canvasImg = [];
							oC2 = [];
							yImg2[i].src = '';
							yImg2[i].onload = function() {}
						}
					}
				}
				yImgLast = [];
				yImgHas = [];
				canvasImg = [];
				var allImgsPre = [];
				var oC2 = [];

				var visibleItemPlaceholderList2 = [];
				for (var i = 0; i < room.array.length; i++) {
					var object1 = new Object();
					object1.currentAllKeywords = room.array[i].currentAllKeywords;
					object1.displayIndex = room.array[i].displayIndex;
					object1.objectArray = room.array[i].objectArray;
					object1.typeId = room.array[i].typeId;
					object1.typeName = room.array[i].typeName;
					visibleItemPlaceholderList2.push(object1);
				};
				for (var i = 0; i < visibleItemPlaceholderList2.length - 1; i++) {
					for (var j = i + 1; j < visibleItemPlaceholderList2.length; j++) {
						if (visibleItemPlaceholderList2[i].displayIndex > visibleItemPlaceholderList2[j].displayIndex) {
							var object = new Object();
							object.currentAllKeywords = visibleItemPlaceholderList2[i].currentAllKeywords;
							object.displayIndex = visibleItemPlaceholderList2[i].displayIndex;
							object.objectArray = visibleItemPlaceholderList2[i].objectArray;
							object.typeId = visibleItemPlaceholderList2[i].typeId;
							object.typeName = visibleItemPlaceholderList2[i].typeName;
							visibleItemPlaceholderList2[i] = visibleItemPlaceholderList2[j];
							visibleItemPlaceholderList2[j] = object;
						};
					}
				}
				for (var i = 0; i < visibleItemPlaceholderList2.length; i++) {
					for (var z = 0; z < allpicstyles.length; z++) {
						for (var x = 0; x < visibleItemPlaceholderList2[i].objectArray.length; x++) {
							if (allpicstyles[z].selectName == visibleItemPlaceholderList2[i].objectArray[x].name) {
								if (hasLoadImg.length > 0) {
									var canload = true;
									for (var y = 0; y < hasLoadImg.length; y++) {
										if (hasLoadImg[y] == visibleItemPlaceholderList2[i].objectArray[x].imageUrl) {
											canload = false;
										}
									}
									if (canload) {
										allImgsPre.push(visibleItemPlaceholderList2[i].objectArray[x].imageUrl);
									}
								} else {
									allImgsPre.push(visibleItemPlaceholderList2[i].objectArray[x].imageUrl);
								}
								break;
							};
						};
					};
				};
				if (hasLoadImg.length > 0) {
					var hasBackgroundImg = false;
					for (var y = 0; y < hasLoadImg.length; y++) {
						if (hasLoadImg[y] == room.backgroundImageUrl) {
							hasBackgroundImg = true;
						}
					}
					if (!hasBackgroundImg) {
						allImgsPre.push(room.backgroundImageUrl);
					}
				} else {
					allImgsPre.push(room.backgroundImageUrl);
				}
				var yImg2Num = 0;
				var yImg2Length = yImg2.length;
				if (allImgsPre.length > 0) {
					for (var i = 0; i < allImgsPre.length; i++) {
						(function(i) {
							yImgLast.push(allImgsPre[i]);
							yImg2[i + yImg2Length] = new Image();
						    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
							yImg2[i + yImg2Length].crossOrigin = '';
						    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


							yImg2[i + yImg2Length].src = allImgsPre[i];
							yImg2[i + yImg2Length].onload = function() {
								//console.log(yImg2[i + yImg2Length].src)
								yImgHas.push(allImgsPre[i]);
								hasLoadImg.push(allImgsPre[i]); //
								yImg2Num++;
								if (yImg2Num == allImgsPre.length) {
									canvas()
								}
							}
							if (yImg2[i + yImg2Length].complete || yImg2[i + yImg2Length].complete === undefined) {
								yImg2[i + yImg2Length].src = allImgsPre[i];
							}
						})(i)

					}
				} else {
					canvas();
				}

				function canvas() {
					canvasImg = [];
					canvasImg.push(room.backgroundImageUrl);
					for (var i = 0; i < visibleItemPlaceholderList2.length; i++) {
						for (var z = 0; z < allpicstyles.length; z++) {
							for (var x = 0; x < visibleItemPlaceholderList2[i].objectArray.length; x++) {
								if (allpicstyles[z].selectName == visibleItemPlaceholderList2[i].objectArray[x].name & allpicstyles[z].display == 'block') {
									canvasImg.push(visibleItemPlaceholderList2[i].objectArray[x].imageUrl);
									break;
								} else if (allpicstyles[z].selectName == visibleItemPlaceholderList2[i].objectArray[x].name & allpicstyles[z].display == 'none') {
									canvasImg.push('');
									break;
								}
							};
						};
					};
					var canvasStartX, canvasStartY;
					document.getElementById('addCanvasContainer').innerHTML = '';
					var oC = document.getElementById('allcanvas');
					oGC = oC.getContext('2d');
					for (var i = 0; i < canvasImg.length; i++) {
						var hiddenCanvas = document.createElement('canvas');
						hiddenCanvas.setAttribute('class', 'hiddenCanvas');
						hiddenCanvas.setAttribute('id', 'hiddenCanvas' + i);
						hiddenCanvas.setAttribute('width', '758');
						hiddenCanvas.setAttribute('height', '568');
						document.getElementById('addCanvasContainer').appendChild(hiddenCanvas);
						oC2[i] = document.getElementById('hiddenCanvas' + i);
						oGC2 = oC2[i].getContext('2d');
						for (var ii = 0; ii < yImg2.length; ii++) {
							if (canvasImg[i] == yImg2[ii].src) {
								oGC.drawImage(yImg2[ii], 0, 0, 758, 568);
								oGC2.drawImage(yImg2[ii], 0, 0, 758, 568);
								var loading = document.getElementById('loading');
								loading.classList.remove("pageIn");
								loading.classList.add("pageOut");
							}
						}
					}
					document.getElementById('allcanvas').onmousedown = function(event) {
						document.onmousemove = function() {}
						document.onmouseup = function() {
							var margin_left;
							var margin_top = 209;
							if (allWidth > 1024) {
								margin_left = (allWidth - 1024) / 2;
							} else {
								margin_left = 0;
							}
							var e = event || window.event;
							var scrollX = document.documentElement.scrollLeft || document.body.scrollLeft;
							var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
							var clickX = e.pageX || e.clientX + scrollX;
							var clickY = e.pageY || e.clientY + scrollY;
							canvasStartX = clickX - margin_left;
							canvasStartY = clickY - margin_top;
							var clickJ = -1;
							for (var j = canvasImg.length - 1; j >= 0; j--) {
								var oGC2 = oC2[j].getContext('2d');
								var oImg = oGC2.getImageData(canvasStartX, canvasStartY, 1, 1);
								var a = oImg.data[3];
								if (a > 1) {
									clickJ = canvasImg.length - j - 1;
									break;
								}
							}
							if (clickJ != -1 && clickJ != canvasImg.length - 1) {
								document.getElementById('diyThingsDetailedContainer').classList.remove('pageIn');
								document.getElementById('diyThingsDetailedContainer').classList.add('pageOut');
								var diyThings = document.querySelectorAll('.diyThings');
								var diyThingsTitleSpan = diyThingsTitle.getElementsByTagName('span')[0];
								diyThingsDetailedContainerShow = false;
								diyThingsTitleSpan.innerHTML = diyThings[clickJ].innerHTML;
								thingsindex[perspectiveListIndex] = clickJ;
								var is = false;
								for (var jj = 0; jj < allstyles.length; jj++) {
									if (allstyles[jj].name == diyThings[clickJ].innerHTML) {
										is = true;
										break;
									};
								}
								if (!is) {
									var styleitem = new Object();
									styleitem.name = diyThings[clickJ].innerHTML;
									allstyles.push(styleitem);
									is = false;
								};
								for (var jj = 0; jj < allpicstyles.length; jj++) {
									if (allpicstyles[jj].name == diyThings[clickJ].innerHTML) {
										is = true;
										break;
									};
								};
								if (!is) {
									var picstyleitem = new Object();
									picstyleitem.name = diyThings[clickJ].innerHTML;
									allpicstyles.push(picstyleitem);
								};

								styles(room);
								picStyles(room);
							}
							document.onmouseup = null;
							document.onmousemove = null;
						}
					}
				}
			}

			var conserveProgramme = document.getElementById('conserveProgramme');
			var styleSave = document.getElementById('styleSave');
			conserveProgramme.addEventListener('click', conserveProgrammeClick);

			function conserveProgrammeClick() { //保存方案
				if (document.getElementById('styleSaveContainer')) {
					var styleSaveContainer = document.getElementById('styleSaveContainer');
					styleSaveContainer.classList.remove('pageOut');
					styleSaveContainer.classList.add('pageIn');
					document.getElementById('closePart3More').onclick = function() {
						styleSaveContainer.classList.remove('pageIn');
						styleSaveContainer.classList.add('pageOut');
					}
				}
				var sendJson = {};
				sendJson.perspectiveListIndex = perspectiveListIndex;
				sendJson.thingsindex = thingsindex;
				sendJson.allstyles = allstyles;
				sendJson.allpicstyles = allpicstyles;
				diyJson = JSON.stringify(sendJson);
				// console.log(diyJson)
			}
		}
	}



	function changeDiyPage() {
		var diyStyleDetailed = document.querySelectorAll('.diyStyleDetailed');
		var diyStyleBottomContainer = document.getElementById('diyStyleBottomContainer');
		var pageNum = -1;
		var currentPage = 0;
		diyStyleBottomContainer.innerHTML = ''; //先把切换页数的按钮组件清空
		for (var j = 0; j < Math.ceil(diyStyleDetailed.length / 6) + 2; j++) {
			pageNum++;
			var changeDiyStylePage = document.createElement('div');
			diyStyleBottomContainer.appendChild(changeDiyStylePage);
			changeDiyStylePage.setAttribute('class', 'changeDiyStylePage');
			changeDiyStylePage.innerHTML = pageNum;
		}
		document.querySelectorAll('.changeDiyStylePage')[0].innerHTML = '上一页';
		document.querySelectorAll('.changeDiyStylePage')[Math.ceil(diyStyleDetailed.length / 6) + 2 - 1].innerHTML = '下一页';
		if (Math.ceil(diyStyleDetailed.length / 6) < 4) {
			diyStyleBottomContainer.style.paddingLeft = (256 - 32 * (Math.ceil(diyStyleDetailed.length / 6)) - 58 * 2) / 2 + 'px';
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
	}
}

function subclick() {
	if (document.getElementById('styleSaveContainer')) {
		document.getElementById('styleSaveContainer').classList.remove('pageIn');
		document.getElementById('styleSaveContainer').classList.add('pageOut');
	}
	diyJson = diyJson;
}