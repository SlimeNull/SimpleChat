import{c as v,d as T,s as w,e as $,n as A,g as C,f as O}from"./forwardRefs-f0d558be.js";import{b as L,u as R,c as x}from"./VOverlay-c00d4473.js";import{g as S,c as h,m as y,T as E,p as D,B as q,l as I,a4 as k,v as V,n as M,G as N,a5 as W}from"./index-7e0ef185.js";import{k as X}from"./index-bc05f82b.js";import{u as Y}from"./tag-1a61e89d.js";const j=D({target:Object},"v-dialog-transition"),z=S()({name:"VDialogTransition",props:j(),setup(n,f){let{slots:e}=f;const l={onBeforeEnter(t){t.style.pointerEvents="none",t.style.visibility="hidden"},async onEnter(t,r){var c;await new Promise(i=>requestAnimationFrame(i)),await new Promise(i=>requestAnimationFrame(i)),t.style.visibility="";const{x:d,y:m,sx:s,sy:a,speed:o}=b(n.target,t),u=v(t,[{transform:`translate(${d}px, ${m}px) scale(${s}, ${a})`,opacity:0},{}],{duration:225*o,easing:T});(c=P(t))==null||c.forEach(i=>{v(i,[{opacity:0},{opacity:0,offset:.33},{}],{duration:225*2*o,easing:w})}),u.finished.then(()=>r())},onAfterEnter(t){t.style.removeProperty("pointer-events")},onBeforeLeave(t){t.style.pointerEvents="none"},async onLeave(t,r){var c;await new Promise(i=>requestAnimationFrame(i));const{x:d,y:m,sx:s,sy:a,speed:o}=b(n.target,t);v(t,[{},{transform:`translate(${d}px, ${m}px) scale(${s}, ${a})`,opacity:0}],{duration:125*o,easing:$}).finished.then(()=>r()),(c=P(t))==null||c.forEach(i=>{v(i,[{},{opacity:0,offset:.2},{opacity:0}],{duration:125*2*o,easing:w})})},onAfterLeave(t){t.style.removeProperty("pointer-events")}};return()=>n.target?h(E,y({name:"dialog-transition"},l,{css:!1}),e):h(E,{name:"dialog-transition"},e)}});function P(n){var e;const f=(e=n.querySelector(":scope > .v-card, :scope > .v-sheet, :scope > .v-list"))==null?void 0:e.children;return f&&[...f]}function b(n,f){const e=n.getBoundingClientRect(),l=A(f),[t,r]=getComputedStyle(f).transformOrigin.split(" ").map(F=>parseFloat(F)),[d,m]=getComputedStyle(f).getPropertyValue("--v-overlay-anchor-origin").split(" ");let s=e.left+e.width/2;d==="left"||m==="left"?s-=e.width/2:(d==="right"||m==="right")&&(s+=e.width/2);let a=e.top+e.height/2;d==="top"||m==="top"?a-=e.height/2:(d==="bottom"||m==="bottom")&&(a+=e.height/2);const o=e.width/l.width,u=e.height/l.height,c=Math.max(1,o,u),i=o/c||0,g=u/c||0,p=l.width*l.height/(window.innerWidth*window.innerHeight),B=p>.12?Math.min(1.5,(p-.12)*10+1):1;return{x:s-(t+l.left),y:a-(r+l.top),sx:i,sy:g,speed:B}}const Z=C("flex-grow-1","div","VSpacer");const G=D({fullscreen:Boolean,retainFocus:{type:Boolean,default:!0},scrollable:Boolean,...L({origin:"center center",scrollStrategy:"block",transition:{component:z},zIndex:2400})},"VDialog"),_=S()({name:"VDialog",props:G(),emits:{"update:modelValue":n=>!0},setup(n,f){let{slots:e}=f;const l=q(n,"modelValue"),{scopeId:t}=R(),r=I();function d(s){var u,c;const a=s.relatedTarget,o=s.target;if(a!==o&&((u=r.value)!=null&&u.contentEl)&&((c=r.value)!=null&&c.globalTop)&&![document,r.value.contentEl].includes(o)&&!r.value.contentEl.contains(o)){const i=W(r.value.contentEl);if(!i.length)return;const g=i[0],p=i[i.length-1];a===g?p.focus():g.focus()}}k&&V(()=>l.value&&n.retainFocus,s=>{s?document.addEventListener("focusin",d):document.removeEventListener("focusin",d)},{immediate:!0}),V(l,async s=>{var a,o;await N(),s?(a=r.value.contentEl)==null||a.focus({preventScroll:!0}):(o=r.value.activatorEl)==null||o.focus({preventScroll:!0})});const m=M(()=>y({"aria-haspopup":"dialog","aria-expanded":String(l.value)},n.activatorProps));return Y(()=>{const[s]=x.filterProps(n);return h(x,y({ref:r,class:["v-dialog",{"v-dialog--fullscreen":n.fullscreen,"v-dialog--scrollable":n.scrollable},n.class],style:n.style},s,{modelValue:l.value,"onUpdate:modelValue":a=>l.value=a,"aria-modal":"true",activatorProps:m.value,role:"dialog"},t),{activator:e.activator,default:function(){for(var a=arguments.length,o=new Array(a),u=0;u<a;u++)o[u]=arguments[u];return h(X,{root:"VDialog"},{default:()=>{var c;return[(c=e.default)==null?void 0:c.call(e,...o)]}})}})}),O({},r)}});export{Z as V,_ as a,z as b};