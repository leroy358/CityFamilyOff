window.addEventListener('load', functionIframe);

function functionIframe() {
	var images = document.querySelectorAll('.images');
	var currentNum = 0;
	var xx = simple.preloader.create();
	xx.concurrentLoad = false;
	document.getElementById('loading').classList.remove('pageOut');
	document.getElementById('loading').classList.add('pageIn');
	for (var i = 0; i < images.length; i++) {
		xx.addItem(images[i].src);
	}
	xx.onComplete = function() {
		document.getElementById('loading').classList.remove('pageIn');
		document.getElementById('loading').classList.add('pageOut');
		document.getElementById('functionIframeContainer').classList.remove('pageOut');
		document.getElementById('functionIframeContainer').classList.add('pageIn');
		images[0].setAttribute('current', '');
		timer1 = setInterval(move, 2500);
	}
	xx.start();

	function move() {
		for (var i = 0; i < images.length; i++) {
			images[i].removeAttribute('current');
		}
		currentNum++;
		if (currentNum >= images.length) {
			currentNum = 0;
		}
		images[currentNum].setAttribute('current', '');
	}
}