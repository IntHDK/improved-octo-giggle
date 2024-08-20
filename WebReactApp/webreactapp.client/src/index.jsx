import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { CookiesProvider } from 'react-cookie';
import App from './main.jsx'
import 'bootstrap/dist/css/bootstrap.min.css';

createRoot(document.getElementById('root')).render(
    <CookiesProvider>
        <StrictMode>
            <App/>
        </StrictMode>
    </CookiesProvider>,
)
