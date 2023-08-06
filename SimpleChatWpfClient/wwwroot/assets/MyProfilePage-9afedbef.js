import{I as Ie,u as o,a as J,v as re,V as le}from"./index-bc05f82b.js";import{V as be}from"./VMain-ffc5ac57.js";import{V as A,a as pe,c as K,b as Ue}from"./VCard-4320fbb2.js";import{f as Ge,V as Q}from"./forwardRefs-f0d558be.js";import{V as xe,a as Te}from"./VDialog-d6d23d6c.js";import{V as G}from"./VBtn-3a8c1ea3.js";import{V as oe,a as T}from"./VRow-8649b514.js";import{V as de}from"./VContainer-e7ac744a.js";import{a as Oe,b as Re,u as Se,d as _e,f as Ne,e as Ae,g as De,V as ke}from"./VTextField-c1b0e354.js";import{V as $e}from"./VFileInput-4670f049.js";import{V as D,b as ne,a as P,c as ze,d as ce,e as Be}from"./VList-c42ec69f.js";import{p as Me,g as qe,B as Le,n as se,l as m,s as He,N as Ee,q as je,v as Y,x as We,V as Xe,c as t,m as fe,F as x,P as Pe,S as Je,W as Ke,G as ve,X as Qe,y as Ye,L as Ze,o as r,a as h,w as a,k as f,u as c,t as V,b as p,d as y,I as $,e as z}from"./index-7e0ef185.js";import{u as et}from"./tag-1a61e89d.js";import{V as he}from"./VMenu-b22d60c4.js";import"./_commonjsHelpers-042e6b4d.js";import"./VOverlay-c00d4473.js";const tt=Me({autoGrow:Boolean,autofocus:Boolean,counter:[Boolean,Number,String],counterValue:Function,prefix:String,placeholder:String,persistentPlaceholder:Boolean,persistentCounter:Boolean,noResize:Boolean,rows:{type:[Number,String],default:5,validator:s=>!isNaN(parseFloat(s))},maxRows:{type:[Number,String],validator:s=>!isNaN(parseFloat(s))},suffix:String,modelModifiers:Object,...Oe(),...Re()},"VTextarea"),at=qe()({name:"VTextarea",directives:{Intersect:Ie},inheritAttrs:!1,props:tt(),emits:{"click:control":s=>!0,"mousedown:control":s=>!0,"update:focused":s=>!0,"update:modelValue":s=>!0},setup(s,k){let{attrs:O,emit:B,slots:g}=k;const v=Le(s,"modelValue"),{isFocused:w,focus:M,blur:R}=Se(s),q=se(()=>typeof s.counterValue=="function"?s.counterValue(v.value):(v.value||"").toString().length),I=se(()=>{if(O.maxlength)return O.maxlength;if(!(!s.counter||typeof s.counter!="number"&&typeof s.counter!="string"))return s.counter});function d(i,_){var n,l;!s.autofocus||!i||(l=(n=_[0].target)==null?void 0:n.focus)==null||l.call(n)}const Z=m(),L=m(),ee=He(""),S=m(),j=se(()=>s.persistentPlaceholder||w.value||s.active);function N(){var i;S.value!==document.activeElement&&((i=S.value)==null||i.focus()),w.value||M()}function ue(i){N(),B("click:control",i)}function te(i){B("mousedown:control",i)}function ie(i){i.stopPropagation(),N(),ve(()=>{v.value="",Qe(s["onClick:clear"],i)})}function ae(i){var n;const _=i.target;if(v.value=_.value,(n=s.modelModifiers)!=null&&n.trim){const l=[_.selectionStart,_.selectionEnd];ve(()=>{_.selectionStart=l[0],_.selectionEnd=l[1]})}}const C=m(),H=m(+s.rows),W=se(()=>["plain","underlined"].includes(s.variant));Ee(()=>{s.autoGrow||(H.value=+s.rows)});function b(){s.autoGrow&&ve(()=>{if(!C.value||!L.value)return;const i=getComputedStyle(C.value),_=getComputedStyle(L.value.$el),n=parseFloat(i.getPropertyValue("--v-field-padding-top"))+parseFloat(i.getPropertyValue("--v-input-padding-top"))+parseFloat(i.getPropertyValue("--v-field-padding-bottom")),l=C.value.scrollHeight,e=parseFloat(i.lineHeight),u=Math.max(parseFloat(s.rows)*e+n,parseFloat(_.getPropertyValue("--v-input-control-height"))),E=parseFloat(s.maxRows)*e+n||1/0,U=Ze(l??0,u,E);H.value=Math.floor((U-n)/e),ee.value=Ye(U)})}je(b),Y(v,b),Y(()=>s.rows,b),Y(()=>s.maxRows,b),Y(()=>s.density,b);let F;return Y(C,i=>{i?(F=new ResizeObserver(b),F.observe(C.value)):F==null||F.disconnect()}),We(()=>{F==null||F.disconnect()}),et(()=>{const i=!!(g.counter||s.counter||s.counterValue),_=!!(i||g.details),[n,l]=Xe(O),[{modelValue:e,...u}]=_e.filterProps(s),[E]=Ne(s);return t(_e,fe({ref:Z,modelValue:v.value,"onUpdate:modelValue":U=>v.value=U,class:["v-textarea v-text-field",{"v-textarea--prefixed":s.prefix,"v-textarea--suffixed":s.suffix,"v-text-field--prefixed":s.prefix,"v-text-field--suffixed":s.suffix,"v-textarea--auto-grow":s.autoGrow,"v-textarea--no-resize":s.noResize||s.autoGrow,"v-text-field--plain-underlined":W.value},s.class],style:s.style},n,u,{centerAffix:H.value===1&&!W.value,focused:w.value}),{...g,default:U=>{let{isDisabled:X,isDirty:me,isReadonly:Ve,isValid:ye}=U;return t(Ae,fe({ref:L,style:{"--v-textarea-control-height":ee.value},onClick:ue,onMousedown:te,"onClick:clear":ie,"onClick:prependInner":s["onClick:prependInner"],"onClick:appendInner":s["onClick:appendInner"],role:"textbox"},E,{active:j.value||me.value,centerAffix:H.value===1&&!W.value,dirty:me.value||s.dirty,disabled:X.value,focused:w.value,error:ye.value===!1}),{...g,default:we=>{let{props:{class:ge,...Fe}}=we;return t(x,null,[s.prefix&&t("span",{class:"v-text-field__prefix"},[s.prefix]),Pe(t("textarea",fe({ref:S,class:ge,value:v.value,onInput:ae,autofocus:s.autofocus,readonly:Ve.value,disabled:X.value,placeholder:s.placeholder,rows:s.rows,name:s.name,onFocus:N,onBlur:R},Fe,l),null),[[Je("intersect"),{handler:d},null,{once:!0}]]),s.autoGrow&&Pe(t("textarea",{class:[ge,"v-textarea__sizer"],"onUpdate:modelValue":Ce=>v.value=Ce,ref:C,readonly:!0,"aria-hidden":"true"},null),[[Ke,v.value]]),s.suffix&&t("span",{class:"v-text-field__suffix"},[s.suffix])])}})},details:_?U=>{var X;return t(x,null,[(X=g.details)==null?void 0:X.call(g,U),i&&t(x,null,[t("span",null,null),t(De,{active:s.persistentCounter||w.value,value:q.value,max:I.value},g.counter)])])}:void 0})}),Ge({},Z,L,S)}}),lt={class:"d-flex align-start"},ot={class:"ms-4"},nt={class:"text-h6"},st={class:"d-flex"},ut={class:"pt-3"},it={class:"mt-8"},rt=f("div",{class:"text-h6"},"好友请求",-1),dt={class:"text-caption"},ct=f("div",{class:"text-h6"},"加群请求",-1),ft={class:"text-caption"},vt=f("div",{class:"text-h6"},"好友列表",-1),mt={class:"text-caption"},gt=f("div",{class:"text-h6"},"群组列表",-1),pt={class:"text-caption"},Rt={__name:"MyProfilePage",setup(s){const k=m(o.tool.getEmptyUserProfile()),O=m([]),B=m([]),g=m([]),v=m([]),w=m([]),M=m([]),R=m([]),q=m(""),I=m(!1),d=m({nickname:"",description:"",avatarFiles:null,avatarPreviewUrl:null});async function Z(){d.value.nickname=k.value.nickname,d.value.description=k.value.description,d.value.avatarFiles=null,d.value.avatarPreviewUrl=k.value.avatar,I.value=!0}async function L(){if(d.value.avatarFiles.length!=0){const n=d.value.avatarFiles[0],l=new FileReader;l.readAsDataURL(n),l.onload=function(){d.value.avatarPreviewUrl=l.result}}}async function ee(){I.value=!0;let n=null;if(d.value.avatarFiles!=null){const l=await o.api.file.upload(d.value.avatarFiles[0]);if(l.isOk)n=l.value.fileUrl;else{o.tool.toast(l.message);return}}o.api.user.setSelfProfile(d.value.nickname,d.value.description,n).then(l=>{l.isOk?(o.tool.toast("已更改个人资料"),I.value=!1,S()):o.tool.toast(l.message)})}async function S(){const n=await o.api.user.getUserProfile(re.state.userId);n.isOk?(k.value=n.value,re.state.userProfile=n.value):o.tool.toast(n.message)}async function j(){return await o.api.user.getFriends().then(n=>{n.isOk?O.value=n.value:o.tool.toast(n.message)})}async function N(){o.api.group.getGroups().then(n=>{n.isOk?B.value=n.value:o.tool.toast(n.message)})}async function ue(){return await o.api.request.getSentFriendRequests().then(async n=>{if(!n.isOk){o.tool.toast(n.message);return}const l=[];for(const e of n.value)await o.api.user.getUserProfile(e.userToId).then(u=>{u.isOk?e.contextProfile=u.value:o.tool.toast(u.message)}),l.push({id:e.id,userFromId:e.userFromId,userToId:e.userToId,contextProfile:e.contextProfile});g.value=l})}async function te(){return await o.api.request.getReceivedFriendRequests().then(async n=>{if(!n.isOk){o.tool.toast(n.message);return}const l=[];for(const e of n.value)console.log(e),await o.api.user.getUserProfile(e.userFromId).then(u=>{u.isOk||o.tool.toast(u.message),e.contextProfile=u.value,l.push({id:e.id,userFromId:e.userFromId,userToId:e.userToId,contextProfile:e.contextProfile})});v.value=l})}async function ie(){return await o.api.request.getSentGroupRequests().then(async n=>{if(!n.isOk){o.tool.toast(n.message);return}const l=[];for(const e of n.value)await o.api.group.getGroupProfile(e.groupToId).then(u=>{u.isOk?e.contextProfile=u.value:o.tool.toast(u.message)}),l.push({id:e.id,userFromId:e.userFromId,userToId:e.userToId,contextProfile:e.contextProfile});w.value=l})}async function ae(){return await o.api.request.getReceivedGroupRequests().then(async n=>{if(!n.isOk){o.tool.toast(n.message);return}const l=[];for(const e of n.value){const u=await o.api.group.getGroupProfile(e.groupToid);if(!u.isOk)continue;const E=await o.api.user.getUserProfile(e.userId);E.isOk&&l.push({id:e.id,userFromId:e.userFromId,userToId:e.userToId,contextUserProfile:E.value,contextGroupProfile:u.value})}M.value=l})}async function C(){let n=null;return R.value.length!=0&&(n=R.value[0].time),o.api.post.getLatestPosts(re.state.userProfile.id,20,n,null).then(l=>{if(l.isOk){const e=l.value;for(const u of e)R.value.unshift(u)}else o.tool.toast(l.message)})}async function H(n){return o.api.post.sendPost(n).then(l=>{l.isOk?(o.tool.toast("已发送"),q.value=""):o.tool.toast(l.message)}).then(()=>C())}async function W(){await S(),await j(),await N(),await ue(),await te(),await ie(),await ae(),await C()}async function b(n){const l=await o.api.request.acceptFriendRequest(n);l.isOk?(o.tool.toast("已接受好友请求"),j(),te()):o.tool.toast(l.message)}async function F(n){const l=await o.api.request.acceptGroupRequest(n);l.isOk?(o.tool.toast("已接受群组请求"),N(),ae()):o.tool.toast(l.message)}async function i(n){await o.api.user.deleteFriend(n).then(l=>{l.isOk?(o.tool.toast("已删除好友"),j()):o.tool.toast(l.message)})}async function _(n){await o.api.group.leaveGroup(n).then(l=>{l.isOk?(o.tool.toast("已离开群组"),N()):o.tool.toast(l.message)})}return W(),(n,l)=>(r(),h(be,null,{default:a(()=>[t(de,null,{default:a(()=>[t(A,null,{default:a(()=>[t(pe,null,{default:a(()=>[f("div",lt,[f("div",null,[t(Q,{class:"elevation-1",size:"x-large"},{default:a(()=>[t(J,{src:c(o).info.getAvatar(k.value.avatar)},null,8,["src"])]),_:1})]),f("div",ot,[f("div",nt,V(c(o).info.getUserFullDisplayName(k.value.userName,k.value.nickname)),1),f("div",null,V(k.value.id),1)]),t(xe),f("div",null,[t(G,{text:"",onClick:l[0]||(l[0]=e=>Z())},{default:a(()=>[p("编辑")]),_:1}),t(Te,{modelValue:I.value,"onUpdate:modelValue":l[7]||(l[7]=e=>I.value=e)},{default:a(()=>[t(oe,{class:"justify-center"},{default:a(()=>[t(T,{cols:"12",sm:"8",md:"6"},{default:a(()=>[t(A,null,{default:a(()=>[t(K,null,{default:a(()=>[p(" 更改个人资料 ")]),_:1}),t(pe,null,{default:a(()=>[f("div",st,[f("div",ut,[t(Q,{size:"x-large",class:"elevation-1"},{default:a(()=>[t(J,{src:c(o).info.getAvatar(d.value.avatarPreviewUrl)},null,8,["src"])]),_:1})]),t(de,{class:"grow-1"},{default:a(()=>[t(oe,null,{default:a(()=>[t(T,{cols:"12",md:"6"},{default:a(()=>[t(ke,{label:"昵称",modelValue:d.value.nickname,"onUpdate:modelValue":l[1]||(l[1]=e=>d.value.nickname=e)},null,8,["modelValue"])]),_:1}),t(T,{cols:"12",md:"6"},{default:a(()=>[t(ke,{label:"描述",modelValue:d.value.description,"onUpdate:modelValue":l[2]||(l[2]=e=>d.value.description=e)},null,8,["modelValue"])]),_:1}),t(T,{cols:"12"},{default:a(()=>[t($e,{label:"头像",modelValue:d.value.avatarFiles,"onUpdate:modelValue":[l[3]||(l[3]=e=>d.value.avatarFiles=e),l[4]||(l[4]=e=>L())],accept:"image/*"},null,8,["modelValue"])]),_:1})]),_:1})]),_:1})])]),_:1}),t(Ue,null,{default:a(()=>[t(xe),t(G,{color:"blue-darken-1",variant:"text",onClick:l[5]||(l[5]=e=>I.value=!1)},{default:a(()=>[p(" 取消 ")]),_:1}),t(G,{color:"blue-darken-1",variant:"text",onClick:l[6]||(l[6]=e=>ee())},{default:a(()=>[p(" 确认 ")]),_:1})]),_:1})]),_:1})]),_:1})]),_:1})]),_:1},8,["modelValue"])])]),f("div",it,V(k.value.description||"这个人很懒，什么都没有留下"),1)]),_:1})]),_:1}),g.value.length!=0||v.value.length!=0?(r(),h(A,{key:0,class:"mt-5"},{default:a(()=>[t(K,null,{default:a(()=>[rt,f("div",dt,"共 "+V(g.value.length+v.value.length)+" 个好友请求",1)]),_:1}),t(D,null,{default:a(()=>[g.value.length!=0?(r(),y(x,{key:0},[t(ne,null,{default:a(()=>[p("发送出的")]),_:1}),(r(!0),y(x,null,$(g.value,e=>(r(),h(P,{key:e.id,title:c(o).info.getUserFullDisplayName(e.contextProfile.userName,e.contextProfile.nickname),subtitle:e.contextProfile.id,"prepend-avatar":c(o).info.getAvatar(e.contextProfile.avatar),onClick:u=>c(o).router.goToUserProfile(e.contextProfile.id)},null,8,["title","subtitle","prepend-avatar","onClick"]))),128))],64)):z("",!0),v.value.length!=0?(r(),y(x,{key:1},[t(ne,null,{default:a(()=>[p("接收到的")]),_:1}),(r(!0),y(x,null,$(v.value,e=>(r(),h(P,{key:e.id,title:c(o).info.getUserFullDisplayName(e.contextProfile.userName,e.contextProfile.nickname),subtitle:e.contextProfile.id},{prepend:a(()=>[t(Q,null,{default:a(()=>[t(J,{src:c(o).info.getAvatar(e.contextProfile.avatar)},null,8,["src"])]),_:2},1024)]),append:a(()=>[t(G,{icon:"",size:"small",flat:"",onClick:u=>b(e.id)},{default:a(()=>[t(le,{color:"primary"},{default:a(()=>[p("mdi-checkbox-marked-circle")]),_:1})]),_:2},1032,["onClick"])]),_:2},1032,["title","subtitle"]))),128))],64)):z("",!0)]),_:1})]),_:1})):z("",!0),w.value.length!=0||M.value.length!=0?(r(),h(A,{key:1,class:"mt-5"},{default:a(()=>[t(K,null,{default:a(()=>[ct,f("div",ft,"共 "+V(g.value.length+v.value.length)+" 个加群请求",1)]),_:1}),t(D,null,{default:a(()=>[w.value.length!=0?(r(),y(x,{key:0},[t(ne,null,{default:a(()=>[p("发送出的")]),_:1}),(r(!0),y(x,null,$(w.value,e=>(r(),h(P,{key:e.id,title:e.contextProfile.name,subtitle:e.contextProfile.id,"prepend-avatar":c(o).info.getAvatar(e.contextProfile.avatar),onClick:u=>c(o).router.goToGroupProfile(e.contextProfile.id)},null,8,["title","subtitle","prepend-avatar","onClick"]))),128))],64)):z("",!0),M.value.length!=0?(r(),y(x,{key:1},[t(ne,null,{default:a(()=>[p("接收到的")]),_:1}),(r(!0),y(x,null,$(M.value,e=>(r(),h(P,{key:e.id},{prepend:a(()=>[t(Q,null,{default:a(()=>[t(J,{src:c(o).info.getAvatar(e.contextUserProfile.avatar)},null,8,["src"])]),_:2},1024),t(Q,null,{default:a(()=>[t(J,{src:c(o).info.getAvatar(e.contextGroupProfile.avatar)},null,8,["src"])]),_:2},1024)]),append:a(()=>[t(G,{icon:"",size:"small",flat:"",onClick:u=>F(e.id)},{default:a(()=>[t(le,{color:"primary"},{default:a(()=>[p("mdi-checkbox-marked-circle")]),_:1})]),_:2},1032,["onClick"])]),default:a(()=>[t(ce,null,{default:a(()=>[p(V(c(o).info.getUserDisplayName(e.contextUserProfile.userName,e.contextUserProfile.nickname))+" 请求加入 "+V(e.contextGroupProfile.name),1)]),_:2},1024)]),_:2},1024))),128))],64)):z("",!0)]),_:1})]),_:1})):z("",!0),t(oe,{class:"mt-5"},{default:a(()=>[t(T,{cols:"3"},{default:a(()=>[t(A,null,{default:a(()=>[t(K,null,{default:a(()=>[vt,f("div",mt,"共 "+V(O.value.length)+" 个好友",1)]),_:1}),t(D,null,{default:a(()=>[(r(!0),y(x,null,$(O.value,e=>(r(),h(P,{key:e.id,title:c(o).info.getUserFullDisplayName(e.userName,e.nickname),subtitle:e.id,"prepend-avatar":c(o).info.getAvatar(e.avatar),onClick:u=>c(o).router.goToUserProfile(e.id)},{append:a(()=>[t(G,{icon:"",size:"x-small",flat:""},{default:a(()=>[t(le,{icon:"mdi-dots-horizontal"}),t(he,{activator:"parent"},{default:a(()=>[t(D,null,{default:a(()=>[t(P,{onClick:u=>i(e.id)},{default:a(()=>[t(ce,null,{default:a(()=>[p("删除好友")]),_:1})]),_:2},1032,["onClick"])]),_:2},1024)]),_:2},1024)]),_:2},1024)]),_:2},1032,["title","subtitle","prepend-avatar","onClick"]))),128))]),_:1})]),_:1}),t(A,{class:"mt-5"},{default:a(()=>[t(K,null,{default:a(()=>[gt,f("div",pt,"共 "+V(B.value.length)+" 个群组",1)]),_:1}),t(D,null,{default:a(()=>[(r(!0),y(x,null,$(B.value,e=>(r(),h(P,{key:e.id,title:e.name,subtitle:e.id,"prepend-avatar":c(o).info.getAvatar(e.avatar),onClick:u=>c(o).router.goToGroupProfile(e.id)},{append:a(()=>[t(G,{icon:"",size:"x-small",flat:""},{default:a(()=>[t(le,{icon:"mdi-dots-horizontal"}),t(he,{activator:"parent"},{default:a(()=>[t(D,null,{default:a(()=>[t(P,{onClick:u=>_(e.id)},{default:a(()=>[t(ce,null,{default:a(()=>[p("退出群组")]),_:1})]),_:2},1032,["onClick"])]),_:2},1024)]),_:2},1024)]),_:2},1024)]),_:2},1032,["title","subtitle","prepend-avatar","onClick"]))),128))]),_:1})]),_:1})]),_:1}),t(T,{cols:"9"},{default:a(()=>[t(A,{title:"说说"},{default:a(()=>[t(D,null,{default:a(()=>[t(P,null,{default:a(()=>[t(de,null,{default:a(()=>[t(oe,null,{default:a(()=>[t(T,{cols:"10"},{default:a(()=>[t(at,{variant:"solo",modelValue:q.value,"onUpdate:modelValue":l[8]||(l[8]=e=>q.value=e)},null,8,["modelValue"])]),_:1}),t(T,{cols:"2"},{default:a(()=>[t(G,{block:"",onClick:l[9]||(l[9]=e=>H(q.value))},{default:a(()=>[p("Post")]),_:1})]),_:1})]),_:1})]),_:1})]),_:1}),t(ze),(r(!0),y(x,null,$(R.value,e=>(r(),h(P,{key:e.id,class:"mx-3 my-5"},{default:a(()=>[t(Be,null,{default:a(()=>[p(V(e.time),1)]),_:2},1024),f("div",null,V(e.content),1)]),_:2},1024))),128)),R.value.length==0?(r(),h(P,{key:0,title:"你来到了没有人的荒地..."})):z("",!0)]),_:1})]),_:1})]),_:1})]),_:1})]),_:1})]),_:1}))}};export{Rt as default};