import { useEffect, useState } from 'react';
import { useCookies } from 'react-cookie';
import { BrowserRouter, Routes, Route} from 'react-router-dom';
import HeaderComponent from './component/headercomponent.jsx';
import LoginPageComponent from './page/loginpagecomponent.jsx';
import LogoutPageComponent from './page/logoutpagecomponent.jsx';
import MainPageComponent from './page/mainpagecomponent.jsx';

function App() {
    const [currentLoginStatus, setCurrentLoginStatus] = useState({
        isLogin: false,
        currentUsername: ""
    });
    const [cookies, setCookie, removeCookie] = useCookies(['AppToken']);

    useEffect(() => {
        loginchecker();
    }, []);

    function updateLoginStatus(appToken) {
        if (appToken) {
            fetchMyLoginInfo(appToken).then((data) => {
                if (data != null) {
                    console.log(data);
                    setCurrentLoginStatus({
                        isLogin: true,
                        currentUsername: data.username
                    })
                }
            })
        } else {
            setCurrentLoginStatus({
                isLogin: false,
                currentUsername: ''
            })
        }
    }

    return (
        <BrowserRouter>
        <div className="container">
            <div className="row">
                <HeaderComponent ref={currentLoginStatus} loginnotifier={loginnotifier} logoutnotifier={logoutnotifier}></HeaderComponent>
            </div>
                <Routes>
                    <Route path="/login" element={<LoginPageComponent loginstatuschecker={loginchecker} loginnotifier={loginnotifier} ref={currentLoginStatus} />}></Route>
                    <Route path="/logout" element={<LogoutPageComponent loginstatuschecker={loginchecker} logoutnotifier={logoutnotifier} ref={currentLoginStatus} />}></Route>
                    <Route path="/register" element={<LoginPageComponent />}></Route>
                    <Route path="/" element={<MainPageComponent />}></Route>
                    <Route path="*" element={<MainPageComponent />}></Route>
                </Routes>
            </div>
        </BrowserRouter>
    );
    async function fetchMyLoginInfo(appToken) {
        var response;
        try {
            response = await fetch('/api/identity', {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    "AppToken": appToken
                }
            });
            if (!response.ok) {
                throw new Error();
            }
            const data = await response.json();

            console.log(data);
            return data;
        } catch {
            removeCookie('AppToken');
            console.log(response.status);
            return null;
        }
    }
    function loginnotifier(apptoken, expiresIn) {
        var dtexp = new Date(expiresIn);
        console.log(dtexp);
        setCookie('AppToken', apptoken);
        updateLoginStatus(apptoken);
    }
    function logoutnotifier() {
        removeCookie('AppToken');
        console.log("cookie removed");
        updateLoginStatus(null);
    }
    function loginchecker() {
        setCurrentLoginStatus({
            isLogin: false,
            currentUsername: ""
        });
        if (cookies.AppToken) {
            console.log("cookie found");
            updateLoginStatus(cookies.AppToken);
        }
    }

}

export default App;