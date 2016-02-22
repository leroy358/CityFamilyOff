/*
	//声明实例
	var sample=simple.reloader.create();
	
	////////////////////////////////////////
	////////////////////////////////////////
	//添加预加载条目
	//sample.addItem(url,resourceType);
	//url:string 资源地址
	//resourceType:string 资源类型，暂且支持两种，image、audio，不声明表示默认使用image
	
	//例：
	//添加图片
	sample.addItem("http://abc.def.jpg","image");
	//添加音频
	sample.addItem("http://abc.def.mp3","audio");
	//不声明类型
	sample.addItem("http://abc.def.jpg");
	
	////////////////////////////////////////
	////////////////////////////////////////
	//设置加载方式
	//sample.concurrentLoad 是否使用并发加载，若为true，则资源列同时请求，若为false，则资源逐一请求，一条请求完成（无论是否请求成功）后才会执行下一条。默认为true。
	
	//例：
	sample.concurrentLoad=false;
	
	////////////////////////////////////////
	////////////////////////////////////////
	//设置加载时的事件
	//sample.onLoad 图片加载成功时都执行此函数，一个传入值：标准img或audio标签加载成功时的执行函数传入值。
	//sample.onError 图片加载失败时执行此函数，一个传入值：标准img或audio标签加载失败时的执行函数传入值。
	//sample.onProgress 图片加载成功或失败都会执行此函数。先执行onLoad或onError函数，再执行此函数。一个传入值，同上。
	//sample.onComplete 所有图片加载完毕（无论成功还是失败）时执行一次此函数。无传入参数。
	
	//例：
	sample.onLoad=function(e){
		alert("成功加载一个图片："+e.target.src);
	};
	
	////////////////////////////////////////
	////////////////////////////////////////
	//其他
	//sample.itemsTotal() 资源条目总数
	//sample.itemsLoaded() 已加载资源条目总数
	//sample.itemsError() 以加载失败资源条目总数
	
	//例：
	sample.onLoad=function(e){
		alert("加载状况："+(sample.itemsLoaded()+sample.itemsError())+"/"+sample.itemsTotal());
	};
	
	////////////////////////////////////////
	////////////////////////////////////////
	//开始加载
	//sample.start() 为了保证已缓存的资源都能正常执行加载时的事件，事件函数必须在开始加载前指定。如果先运行start函数再指定事件，则已缓存的资源可能不会触发事件。
	
	//例：
	sample.start();
	
	
*/

if(typeof(simple)=="undefined"){
	var simple = {};
}

simple.preloader=function(){

	function Preloader(){
		
	//私有变量
		var itemList=[];	
		var itemsTotal=0;
		var itemsLoaded=0;
		var itemsError=0;
		var thisE=this;
		
	//私有函数
		function loadResource(url,resourceType,onLoadedFunc,onErrorFunc){
			var resource;
			if(resourceType=="image"){
				resource=new Image();
			}else if(resourceType=="audio"){
				resource=new Audio();
			}else{
				alert("不支持的资源类型:"+resourceType);
				return;
			}
			
			resource.onload=function(e){
				onLoadedFunc(e);
			}
			resource.onerror=function(e){
				onErrorFunc(e);
			}
			resource.src=url;
		}
			
		function loadResourceAtIndex(index){
			loadResource(itemList[index]["url"],itemList[index]["resourceType"],
				function(e){
					itemsLoaded++;
					if(typeof(thisE.onLoad)=="function")thisE.onLoad(e);
					if(typeof(thisE.onProgress)=="function")thisE.onProgress(e);
					if(itemsTotal==itemsLoaded+itemsError){
						if(typeof(thisE.onComplete)=="function")thisE.onComplete();
					}
				},
				function(e){
					itemsError++;
					if(typeof(thisE.onError)=="function")thisE.onError(e);
					if(typeof(thisE.onProgress)=="function")thisE.onProgress(e);
					if(itemsTotal==itemsLoaded+itemsError){
						if(typeof(thisE.onComplete)=="function")thisE.onComplete();
					}
				}
			);
		}
		
		function loadResourceFromIndex(index){
			loadResource(itemList[index]["url"],itemList[index]["resourceType"],
				function(e){
					itemsLoaded++;
					if(typeof(thisE.onLoad)=="function")thisE.onLoad(e);
					if(typeof(thisE.onProgress)=="function")thisE.onProgress(e);
					if(itemsTotal==itemsLoaded+itemsError){
						if(typeof(thisE.onComplete)=="function")thisE.onComplete();
					}else{
						loadResourceFromIndex(index+1);
					}
				},
				function(e){
					itemsError++;
					if(typeof(thisE.onError)=="function")thisE.onError(e);
					if(typeof(thisE.onProgress)=="function")thisE.onProgress(e);
					if(itemsTotal==itemsLoaded+itemsError){
						if(typeof(thisE.onComplete)=="function")thisE.onComplete();
					}else{
						loadResourceFromIndex(index+1);
					}
				}
			);
		}
	
	//公有变量
		this.concurrentLoad=true;
		this.itemsTotal=function(){return itemsTotal};
		this.itemsLoaded=function(){return itemsLoaded};
		this.itemsError=function(){return itemsError};
		
	
	//公有函数
		//添加条目
		this.addItem=function(url,resourceType){
			if(!resourceType)resourceType="image";
			itemList.push({"url":url,"resourceType":resourceType});
			itemsTotal=itemList.length;
		}
		
		//开始载入
		this.start=function(){
			var i;
			var thisE=this;
			itemsLoaded=0;
			itemsError=0;
			
			if(this.concurrentLoad){//并发加载
				for(i=0;i<itemList.length;i++){
					loadResourceAtIndex(i);
				}
			}else{//依次加载
				loadResourceFromIndex(0);
			}
		};
		
		//进度事件
		this.onProgress;
		
		//成功载入事件
		this.onLoad;
		
		//载入失败事件
		this.onError;
		
		//完成事件
		this.onComplete;
		
	}
	
	function create(){
		return new Preloader();
	}
	
	return {
		create:create
	}

}();