define(["browser","layoutManager","dom","focusManager","scrollStyles"],function(browser,layoutManager,dom,focusManager){"use strict";function type(value){return null==value?String(value):"object"==typeof value||"function"==typeof value?Object.prototype.toString.call(value).match(/\s([a-z]+)/i)[1].toLowerCase()||"object":typeof value}function disableOneEvent(event){event.preventDefault(),event.stopPropagation(),this.removeEventListener(event.type,disableOneEvent)}function within(number,min,max){return number<min?min:number>max?max:number}var dragMouseEvents=["mousemove","mouseup"],dragTouchEvents=["touchmove","touchend"],wheelEvent=document.implementation.hasFeature("Event.wheel","3.0")?"wheel":"mousewheel",interactiveElements=["INPUT","SELECT","TEXTAREA"],abs=Math.abs,sqrt=Math.sqrt,pow=Math.pow,round=Math.round,max=Math.max,scrollerFactory=(Math.min,function(frame,options){function sibling(n,elem){for(var matched=[];n;n=n.nextSibling)1===n.nodeType&&n!==elem&&matched.push(n);return matched}function load(isInit){frameSize=getWidthOrHeight(frameElement,o.horizontal?"width":"height");var slideeSize=o.scrollWidth||Math.max(slideeElement[o.horizontal?"offsetWidth":"offsetHeight"],slideeElement[o.horizontal?"scrollWidth":"scrollHeight"]);pos.start=0,pos.end=max(slideeSize-frameSize,0),isInit||slideTo(within(pos.dest,pos.start,pos.end))}function getWidthOrHeight(elem,name,extra){var valueIsBorderBox=!0,val="width"===name?elem.offsetWidth:elem.offsetHeight,styles=getComputedStyle(elem,null),isBorderBox="border-box"===styles.getPropertyValue("box-sizing");if(val<=0||null==val){if((val<0||null==val)&&(val=elem.style[name]),rnumnonpx.test(val))return val;valueIsBorderBox=isBorderBox&&(support.boxSizingReliable()||val===elem.style[name]),val=parseFloat(val)||0}return val+augmentWidthOrHeight(elem,name,extra||(isBorderBox?"border":"content"),valueIsBorderBox,styles)}function augmentWidthOrHeight(elem,name,extra,isBorderBox,styles){for(var i=extra===(isBorderBox?"border":"content")?4:"width"===name?1:0,val=0;i<4;i+=2);return val}function nativeScrollTo(container,pos,immediate){!immediate&&container.scrollTo?o.horizontal?container.scrollTo(pos,0):container.scrollTo(0,pos):o.horizontal?container.scrollLeft=Math.round(pos):container.scrollTop=Math.round(pos)}function slideTo(newPos,immediate){return newPos=within(newPos,pos.start,pos.end),transform?(animation.from=pos.cur,animation.to=newPos,animation.tweesing=dragging.tweese||dragging.init&&!dragging.slidee,animation.immediate=!animation.tweesing&&(immediate||dragging.init&&dragging.slidee||!o.speed),dragging.tweese=0,void(newPos!==pos.dest&&(pos.dest=newPos,renderAnimate(animation)))):void nativeScrollTo(slideeElement,newPos,immediate)}function renderAnimate(){var obj=getComputedStyle(slideeElement,null).getPropertyValue("transform").match(/([-+]?(?:\d*\.)?\d+)\D*, ([-+]?(?:\d*\.)?\d+)\D*\)/);obj&&(pos.cur=parseInt(o.horizontal?obj[1]:obj[2])*-1);var keyframes;animation.to=round(animation.to),keyframes=o.horizontal?[{transform:"translate3d("+-round(pos.cur||animation.from)+"px, 0, 0)",offset:0},{transform:"translate3d("+-round(animation.to)+"px, 0, 0)",offset:1}]:[{transform:"translate3d(0, "+-round(pos.cur||animation.from)+"px, 0)",offset:0},{transform:"translate3d(0, "+-round(animation.to)+"px, 0)",offset:1}];var speed=o.speed;animation.immediate&&(speed=o.immediateSpeed||50,browser.animate||(o.immediateSpeed=0));var animationConfig={duration:speed,iterations:1,fill:"both"};browser.animate&&(animationConfig.easing="ease-out");var animationInstance=slideeElement.animate(keyframes,animationConfig);animationInstance.onfinish=function(){pos.cur=animation.to,document.dispatchEvent(scrollEvent)}}function getBoundingClientRect(elem){return elem.getBoundingClientRect?elem.getBoundingClientRect():{top:0,left:0}}function to(location,item,immediate){if("boolean"===type(item)&&(immediate=item,item=void 0),void 0===item)slideTo(pos[location],immediate);else{var itemPos=self.getPos(item);itemPos&&slideTo(itemPos[location],immediate,!0)}}function continuousInit(source){dragging.released=0,dragging.source=source,dragging.slidee="slidee"===source}function dragInitSlidee(event){dragInit(event,"slidee")}function dragInit(event,source){var isTouch="touchstart"===event.type,isSlidee="slidee"===source;if(!(dragging.init||!isTouch&&isInteractive(event.target))&&(!isSlidee||(isTouch?o.touchDragging:o.mouseDragging&&event.which<2))){isTouch||event.preventDefault(),continuousInit(source),dragging.init=0,dragging.source=event.target,dragging.touch=isTouch;var pointer=isTouch?event.touches[0]:event;dragging.initX=pointer.pageX,dragging.initY=pointer.pageY,dragging.initPos=isSlidee?pos.cur:hPos.cur,dragging.start=+new Date,dragging.time=0,dragging.path=0,dragging.delta=0,dragging.locked=0,dragging.pathToLock=isSlidee?isTouch?30:10:0,isTouch?dragTouchEvents.forEach(function(eventName){dom.addEventListener(document,eventName,dragHandler,{passive:!0})}):transform&&dragMouseEvents.forEach(function(eventName){dom.addEventListener(document,eventName,dragHandler,{passive:!0})}),isSlidee&&slideeElement.classList.add(o.draggedClass)}}function dragHandler(event){dragging.released="mouseup"===event.type||"touchend"===event.type;var pointer=dragging.touch?event[dragging.released?"changedTouches":"touches"][0]:event;if(dragging.pathX=pointer.pageX-dragging.initX,dragging.pathY=pointer.pageY-dragging.initY,dragging.path=sqrt(pow(dragging.pathX,2)+pow(dragging.pathY,2)),dragging.delta=o.horizontal?dragging.pathX:dragging.pathY,dragging.released||!(dragging.path<1)){if(!dragging.init){if(dragging.path<o.dragThreshold)return dragging.released?dragEnd():void 0;if(!(o.horizontal?abs(dragging.pathX)>abs(dragging.pathY):abs(dragging.pathX)<abs(dragging.pathY)))return dragEnd();dragging.init=1}event.preventDefault(),!dragging.locked&&dragging.path>dragging.pathToLock&&dragging.slidee&&(dragging.locked=1,dragging.source.addEventListener("click",disableOneEvent)),dragging.released&&dragEnd(),slideTo(dragging.slidee?round(dragging.initPos-dragging.delta):handleToSlidee(dragging.initPos+dragging.delta))}}function dragEnd(){dragging.released=!0,dragging.touch?dragTouchEvents.forEach(function(eventName){dom.removeEventListener(document,eventName,dragHandler,{passive:!0})}):dragMouseEvents.forEach(function(eventName){dom.removeEventListener(document,eventName,dragHandler,{passive:!0})}),dragging.slidee&&slideeElement.classList.remove(o.draggedClass),setTimeout(function(){dragging.source.removeEventListener("click",disableOneEvent)}),dragging.init=0}function isInteractive(element){for(;element;){if(interactiveElements.indexOf(element.tagName)!==-1)return!0;element=element.parentNode}return!1}function normalizeWheelDelta(event){return scrolling.curDelta=(o.horizontal?event.deltaY||event.deltaX:event.deltaY)||-event.wheelDelta,transform&&(scrolling.curDelta/=1===event.deltaMode?3:100),scrolling.curDelta}function scrollHandler(event){if(o.scrollBy&&pos.start!==pos.end){var delta=normalizeWheelDelta(event);transform?(delta>0&&pos.dest<pos.end||delta<0&&pos.dest>pos.start,self.slideBy(o.scrollBy*delta)):(isSmoothScrollSupported&&(delta*=12),o.horizontal?slideeElement.scrollLeft+=delta:slideeElement.scrollTop+=delta)}}function onResize(){load(!1)}function resetScroll(){o.horizontal?this.scrollLeft=0:this.scrollTop=0}function onFrameClick(e){if(1===e.which){var focusableParent=focusManager.focusableParent(e.target);focusableParent&&focusableParent!==document.activeElement&&focusableParent.focus()}}var o=Object.assign({},{slidee:null,horizontal:!1,scrollSource:null,scrollBy:0,scrollHijack:300,dragSource:null,mouseDragging:1,touchDragging:1,swingSpeed:.2,dragThreshold:3,intervactive:null,speed:0,draggedClass:"dragged",activeClass:"active",disabledClass:"disabled"},options),isSmoothScrollSupported="scrollBehavior"in document.documentElement.style;isSmoothScrollSupported&&browser.firefox?options.enableNativeScroll=!0:options.requireAnimation?options.enableNativeScroll=!1:layoutManager.tv&&browser.animate||(options.enableNativeScroll=!0);var self=this;self.options=o;var frameElement=frame,slideeElement=o.slidee?o.slidee:sibling(frameElement.firstChild)[0],frameSize=0,pos={start:0,center:0,end:0,cur:0,dest:0},transform=!options.enableNativeScroll,hPos={start:0,end:0,cur:0},scrollSource=o.scrollSource?o.scrollSource:frameElement,dragSourceElement=o.dragSource?o.dragSource:frameElement,animation={},dragging={released:1},scrolling={last:0,delta:0,resetTime:200};frame=frameElement,self.initialized=0,self.frame=frame,self.slidee=slideeElement,self.options=o,self.dragging=dragging;var pnum=/[+-]?(?:\d*\.|)\d+(?:[eE][+-]?\d+|)/.source,rnumnonpx=new RegExp("^("+pnum+")(?!px)[a-z%]+$","i");self.reload=function(){load()};var scrollEvent=new CustomEvent("scroll");self.getPos=function(item){var slideeOffset=getBoundingClientRect(slideeElement),itemOffset=getBoundingClientRect(item),offset=o.horizontal?itemOffset.left-slideeOffset.left:itemOffset.top-slideeOffset.top,size=o.horizontal?itemOffset.width:itemOffset.height;size||0===size||(size=item[o.horizontal?"offsetWidth":"offsetHeight"]);var centerOffset=o.centerOffset||0;return transform||(centerOffset=0,offset+=o.horizontal?slideeElement.scrollLeft:slideeElement.scrollTop),{start:offset,center:offset+centerOffset-frameSize/2+size/2,end:offset-frameSize+size,size:size}},self.getCenterPosition=function(item){var pos=self.getPos(item);return within(pos.center,pos.start,pos.end)},self.slideBy=function(delta,immediate){delta&&slideTo(pos.dest+delta,immediate)},self.slideTo=function(pos,immediate){slideTo(pos,immediate)},self.toStart=function(item,immediate){to("start",item,immediate)},self.toEnd=function(item,immediate){to("end",item,immediate)},self.toCenter=function(item,immediate){to("center",item,immediate)},self.destroy=function(){return dom.removeEventListener(window,"resize",onResize,{passive:!0}),dom.removeEventListener(frameElement,"scroll",resetScroll,{passive:!0}),dom.removeEventListener(scrollSource,wheelEvent,scrollHandler,{passive:!0}),dom.removeEventListener(dragSourceElement,"touchstart",dragInitSlidee,{passive:!0}),dom.removeEventListener(frameElement,"click",onFrameClick,{passive:!0,capture:!0}),dom.removeEventListener(dragSourceElement,"mousedown",dragInitSlidee,{}),self.initialized=0,self},self.init=function(){if(!self.initialized){if(frame.sly)throw new Error("There is already a Sly instance on this element");frame.sly=!0;var movables=[];return slideeElement&&movables.push(slideeElement),transform?(slideeElement.style["will-change"]="transform",o.horizontal?slideeElement.classList.add("animatedScrollX"):slideeElement.classList.add("animatedScrollY")):o.horizontal?layoutManager.desktop?slideeElement.classList.add("smoothScrollX"):slideeElement.classList.add("hiddenScrollX"):layoutManager.desktop?slideeElement.classList.add("smoothScrollY"):slideeElement.classList.add("hiddenScrollY"),(o.horizontal||transform)&&dom.addEventListener(dragSourceElement,"mousedown",dragInitSlidee,{}),transform?(dom.addEventListener(dragSourceElement,"touchstart",dragInitSlidee,{passive:!0}),o.scrollWidth||dom.addEventListener(window,"resize",onResize,{passive:!0}),o.horizontal||dom.addEventListener(frameElement,"scroll",resetScroll,{passive:!0}),dom.addEventListener(scrollSource,wheelEvent,scrollHandler,{passive:!0})):o.horizontal&&dom.addEventListener(scrollSource,wheelEvent,scrollHandler,{passive:!0}),dom.addEventListener(frameElement,"click",onFrameClick,{passive:!0,capture:!0}),self.initialized=1,load(!0),self}}});return scrollerFactory.create=function(frame,options){var instance=new scrollerFactory(frame,options);return Promise.resolve(instance)},scrollerFactory});