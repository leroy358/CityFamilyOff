window.addEventListener("load", pics);
function pics(){
	var currentNum=0;
	var pics=document.querySelectorAll('.pics');
	var houseTypeChange_previous_fengge=document.getElementById('houseTypeChange_previous_fengge');
	var houseTypeChange_next_fengge=document.getElementById('houseTypeChange_next_fengge');
	houseTypeChange_previous_fengge.onclick=function(){
		currentNum=currentNum-1<0?0:currentNum-1;
		for(var i=0;i<pics.length;i++){
			pics[i].classList.remove('pageIn');
			pics[i].classList.add('pageOut');
		}
		pics[currentNum].classList.remove('pageOut');
		pics[currentNum].classList.add('pageIn');
	}
	houseTypeChange_next_fengge.onclick=function(){
		currentNum=currentNum+1>pics.length-1?pics.length-1:currentNum+1;
		for(var i=0;i<pics.length;i++){
			pics[i].classList.remove('pageIn');
			pics[i].classList.add('pageOut');
		}
		pics[currentNum].classList.remove('pageOut');
		pics[currentNum].classList.add('pageIn');
	}
}