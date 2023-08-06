/**
 * main.js
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Components
import App from './App.vue'

// Composables
import { createApp } from 'vue'

// Plugins
import { registerPlugins } from '@/plugins'

// Polyfill
// import PolyfillEventSource from 'eventsource-polyfill'
// window.EventSource = PolyfillEventSource;

const app = createApp(App)

registerPlugins(app)

app.mount('#app')
