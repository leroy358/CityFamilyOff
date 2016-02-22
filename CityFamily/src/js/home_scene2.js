window.addEventListener("load", pics);
function pics(){
	var currentNum=0;
	var pics=document.querySelectorAll('.pics');
	var homeScene_previous=document.getElementById('homeScene_previous');
	var homeScene_next=document.getElementById('homeScene_next');
	homeScene_previous.onclick=function(){
		currentNum=currentNum-1<0?0:currentNum-1;
		for(var i=0;i<pics.length;i++){
			pics[i].classList.remove('pageIn');
			pics[i].classList.add('pageOut');
		}
		pics[currentNum].classList.remove('pageOut');
		pics[currentNum].classList.add('pageIn');
	}
	homeScene_next.onclick=function(){
		currentNum=currentNum+1>pics.length-1?pics.length-1:currentNum+1;
		for(var i=0;i<pics.length;i++){
			pics[i].classList.remove('pageIn');
			pics[i].classList.add('pageOut');
		}
		pics[currentNum].classList.remove('pageOut');
		pics[currentNum].classList.add('pageIn');
	}
}