import{R as Y,f as Q,V as Z,c as R,d as ee,g as ae,h as ne,v as te,u as i}from"./index-bc05f82b.js";import{V as le}from"./VMain-ffc5ac57.js";import{m as N,d as W,u as O}from"./tag-1a61e89d.js";import{p as z,f as se,g as D,h as oe,n as _,E as ie,z as j,c as n,O as q,P as H,Q as de,R as G,S as ue,U as ce,l as P,o as m,a as y,w as o,d as T,I as U,F as w,b as V,e as J,k as F,u as A}from"./index-7e0ef185.js";import{m as re,u as ve,a as me,b as fe,V as g}from"./VBtn-3a8c1ea3.js";import{a as pe}from"./forwardRefs-f0d558be.js";import{m as K,a as xe}from"./VOverlay-c00d4473.js";import{V as $,a as C,d as ge}from"./VList-c42ec69f.js";import{V as ke}from"./VContainer-e7ac744a.js";import{V as M}from"./VTooltip-d7ce11d9.js";import"./_commonjsHelpers-042e6b4d.js";const E=Symbol.for("vuetify:v-expansion-panel"),ye=["default","accordion","inset","popout"],Ve=z({color:String,variant:{type:String,default:"default",validator:e=>ye.includes(e)},readonly:Boolean,...N(),...re(),...W(),...se()},"VExpansionPanels"),_e=D()({name:"VExpansionPanels",props:Ve(),emits:{"update:modelValue":e=>!0},setup(e,r){let{slots:s}=r;ve(e,E);const{themeClasses:a}=oe(e),u=_(()=>e.variant&&`v-expansion-panels--variant-${e.variant}`);return ie({VExpansionPanel:{color:j(e,"color")},VExpansionPanelTitle:{readonly:j(e,"readonly")}}),O(()=>n(e.tag,{class:["v-expansion-panels",a.value,u.value,e.class],style:e.style},s)),{}}}),Pe=z({...N(),...K()},"VExpansionPanelText"),h=D()({name:"VExpansionPanelText",props:Pe(),setup(e,r){let{slots:s}=r;const a=q(E);if(!a)throw new Error("[Vuetify] v-expansion-panel-text needs to be placed inside v-expansion-panel");const{hasContent:u,onAfterLeave:p}=xe(e,a.isSelected);return O(()=>n(pe,{onAfterLeave:p},{default:()=>{var f;return[H(n("div",{class:["v-expansion-panel-text",e.class],style:e.style},[s.default&&u.value&&n("div",{class:"v-expansion-panel-text__wrapper"},[(f=s.default)==null?void 0:f.call(s)])]),[[de,a.isSelected.value]])]}})),{}}}),X=z({color:String,expandIcon:{type:G,default:"$expand"},collapseIcon:{type:G,default:"$collapse"},hideActions:Boolean,ripple:{type:[Boolean,Object],default:!1},readonly:Boolean,...N()},"VExpansionPanelTitle"),Ce=D()({name:"VExpansionPanelTitle",directives:{Ripple:Y},props:X(),setup(e,r){let{slots:s}=r;const a=q(E);if(!a)throw new Error("[Vuetify] v-expansion-panel-title needs to be placed inside v-expansion-panel");const{backgroundColorClasses:u,backgroundColorStyles:p}=Q(e,"color"),f=_(()=>({collapseIcon:e.collapseIcon,disabled:a.disabled.value,expanded:a.isSelected.value,expandIcon:e.expandIcon,readonly:e.readonly}));return O(()=>{var k;return H(n("button",{class:["v-expansion-panel-title",{"v-expansion-panel-title--active":a.isSelected.value},u.value,e.class],style:[p.value,e.style],type:"button",tabindex:a.disabled.value?-1:void 0,disabled:a.disabled.value,"aria-expanded":a.isSelected.value,onClick:e.readonly?void 0:a.toggle},[n("span",{class:"v-expansion-panel-title__overlay"},null),(k=s.default)==null?void 0:k.call(s,f.value),!e.hideActions&&n("span",{class:"v-expansion-panel-title__icon"},[s.actions?s.actions(f.value):n(Z,{icon:a.isSelected.value?e.collapseIcon:e.expandIcon},null)])]),[[ue("ripple"),e.ripple]])}),{}}}),he=z({title:String,text:String,bgColor:String,...N(),...R(),...me(),...K(),...ee(),...W(),...X()},"VExpansionPanel"),B=D()({name:"VExpansionPanel",props:he(),emits:{"group:selected":e=>!0},setup(e,r){let{slots:s}=r;const a=fe(e,E),{backgroundColorClasses:u,backgroundColorStyles:p}=Q(e,"bgColor"),{elevationClasses:f}=ae(e),{roundedClasses:k}=ne(e),b=_(()=>(a==null?void 0:a.disabled.value)||e.disabled),I=_(()=>a.group.items.value.reduce((v,c,t)=>(a.group.selected.value.includes(c.id)&&v.push(t),v),[])),L=_(()=>{const v=a.group.items.value.findIndex(c=>c.id===a.id);return!a.isSelected.value&&I.value.some(c=>c-v===1)}),S=_(()=>{const v=a.group.items.value.findIndex(c=>c.id===a.id);return!a.isSelected.value&&I.value.some(c=>c-v===-1)});return ce(E,a),O(()=>{const v=!!(s.text||e.text),c=!!(s.title||e.title);return n(e.tag,{class:["v-expansion-panel",{"v-expansion-panel--active":a.isSelected.value,"v-expansion-panel--before-active":L.value,"v-expansion-panel--after-active":S.value,"v-expansion-panel--disabled":b.value},k.value,u.value,e.class],style:[p.value,e.style]},{default:()=>{var t;return[n("div",{class:["v-expansion-panel__shadow",...f.value]},null),c&&n(Ce,{key:"title",collapseIcon:e.collapseIcon,color:e.color,expandIcon:e.expandIcon,hideActions:e.hideActions,ripple:e.ripple},{default:()=>[s.title?s.title():e.title]}),v&&n(h,{key:"text",eager:e.eager},{default:()=>[s.text?s.text():e.text]}),(t=s.default)==null?void 0:t.call(s)]}})}),{}}}),Ee={class:"d-flex justify-end mt-5"},be={class:"d-flex justify-end mt-5"},Ie={class:"d-flex justify-end mt-5"},Le={__name:"AdminPage",setup(e){console.log(`admin page, token: ${te.state.JWT}`);const r=P([]),s=P([]),a=P([]),u=P([]),p=P([]);async function f(){return await i.api.manage.getUsersNeedActive().then(t=>{t.isOk?s.value=t.value:i.tool.toast(t.message)})}async function k(){return await i.api.manage.getUsersBanned().then(t=>{t.isOk?a.value=t.value:i.tool.toast(t.message)})}async function b(){return await i.api.manage.getUsersIsAdmin().then(t=>{t.isOk?u.value=t.value:i.tool.toast(t.message)})}async function I(){return await i.api.manage.getAllUsers().then(t=>{t.isOk?p.value=t.value:i.tool.toast(t.message)})}async function L(t){return await i.api.manage.activeUser(t.id).then(d=>{d.isOk?(i.tool.toast(`用户 ${t.userName} 已激活`),s.value=s.value.filter(l=>l.id!==t.id)):i.tool.toast(d.message)})}async function S(t,d){return i.api.manage.setUserBan(t.id,d).then(l=>{l.isOk?(i.tool.toast(`设置完成! 用户 ${t.userName} 当前封禁状态: ${d}`),d?a.value.push(t):a.value=a.value.filter(x=>x.id!==t.id)):i.tool.toast(l.message)})}async function v(t,d){return i.api.manage.setUserAdmin(t.id,d).then(l=>{l.isOk?(i.tool.toast(`设置完成! 用户 ${t.userName} 当前管理员状态: ${d}`),d?u.value.push(t):u.value=u.value.filter(x=>x.id!==t.id)):i.tool.toast(l.message)})}async function c(){await f(),await k(),await b(),await I(),s.value.length>0&&r.value.push(0)}return c(),(t,d)=>(m(),y(le,null,{default:o(()=>[n(ke,null,{default:o(()=>[n(_e,{modelValue:r.value,"onUpdate:modelValue":d[3]||(d[3]=l=>r.value=l)},{default:o(()=>[n(B,{title:"等待激活的用户"},{default:o(()=>[n(h,null,{default:o(()=>[n($,null,{default:o(()=>[(m(!0),T(w,null,U(s.value,l=>(m(),y(C,{key:l.id,title:A(i).info.getUserFullDisplayName(l.userName,l.nickname)},{append:o(()=>[n(g,{icon:"mdi-checkbox-marked-circle",onClick:x=>L(l),size:"small"},null,8,["onClick"])]),_:2},1032,["title"]))),128)),s.value.length===0?(m(),y(C,{key:0},{default:o(()=>[n(ge,null,{default:o(()=>[V("没有需要激活的用户")]),_:1})]),_:1})):J("",!0)]),_:1}),F("div",Ee,[n(g,{onClick:d[0]||(d[0]=l=>f())},{default:o(()=>[V("刷新")]),_:1})])]),_:1})]),_:1}),a.value.length!=0?(m(),y(B,{key:0,title:"被封禁的用户"},{default:o(()=>[n(h,null,{default:o(()=>[n($,null,{default:o(()=>[(m(!0),T(w,null,U(a.value,l=>(m(),y(C,{key:l.id,title:A(i).info.getUserFullDisplayName(l.userName,l.nickname)},{append:o(()=>[n(g,{icon:"mdi-cancel",onClick:x=>S(l,!1),size:"small"},null,8,["onClick"])]),_:2},1032,["title"]))),128))]),_:1}),F("div",be,[n(g,{onClick:d[1]||(d[1]=l=>k())},{default:o(()=>[V("刷新")]),_:1})])]),_:1})]),_:1})):J("",!0),n(B,{title:"管理员用户"},{default:o(()=>[n(h,null,{default:o(()=>[n($,null,{default:o(()=>[(m(!0),T(w,null,U(u.value,l=>(m(),y(C,{key:l.id,title:A(i).info.getUserFullDisplayName(l.userName,l.nickname)},{append:o(()=>[n(g,{icon:"mdi-cancel",onClick:x=>v(l,!1),size:"small"},null,8,["onClick"])]),_:2},1032,["title"]))),128))]),_:1}),F("div",Ie,[n(g,{onClick:d[2]||(d[2]=l=>b())},{default:o(()=>[V("刷新")]),_:1})])]),_:1})]),_:1}),n(B,{title:"所有用户"},{default:o(()=>[n(h,null,{default:o(()=>[n($,null,{default:o(()=>[(m(!0),T(w,null,U(p.value,l=>(m(),y(C,{key:l.id,title:A(i).info.getUserFullDisplayName(l.userName,l.nickname)},{append:o(()=>[n(g,{icon:"mdi-account-key",color:"green",onClick:x=>v(l,!0),size:"small"},{default:o(()=>[n(M,{activator:"parent",location:"bottom"},{default:o(()=>[V("设置为管理员")]),_:1})]),_:2},1032,["onClick"]),n(g,{icon:"mdi-account-cancel",color:"red",onClick:x=>S(l,!0),size:"small",class:"ms-2"},{default:o(()=>[n(M,{activator:"parent",location:"bottom"},{default:o(()=>[V("封禁用户")]),_:1})]),_:2},1032,["onClick"])]),_:2},1032,["title"]))),128))]),_:1})]),_:1})]),_:1})]),_:1},8,["modelValue"])]),_:1})]),_:1}))}};export{Le as default};
