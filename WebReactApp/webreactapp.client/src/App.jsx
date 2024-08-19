import { useEffect, useState } from 'react';
import { useCookies } from 'react-cookie';
import LoginedStatusComponent from './LoginedStatusComponent.jsx'

function App() {
    const [forecasts, setForecasts] = useState();
    const [currentLoginStatus, setCurrentLoginStatus] = useState({
        isLogin: false,
        currentUsername: ""
    });
    const [cookies, setCookie, removeCookie] = useCookies(['AppToken']);

    useEffect(() => {
        setCurrentLoginStatus({
            isLogin: false,
            currentUsername: ""
        });
        populateWeatherData();
        if (cookies.AppToken) {
            updateLoginStatus(cookies.AppToken);
        }
    }, []);

    function updateLoginStatus(appToken) {
        if (appToken) {
            fetchMyLoginInfo(appToken).then((data) => {
                if (data != null) {
                    console.log(data);
                    setCurrentLoginStatus({
                        isLogin: true,
                        currentUsername: data.Username
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

    const contents = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div className="container">
            <div className="row">
                <LoginedStatusComponent ref={currentLoginStatus} loginnotifier={loginnotifier} logoutnotifier={logoutnotifier}></LoginedStatusComponent>
            </div>
            <div className="row">
                <h1 id="tableLabel">Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        </div>
    );
    
    async function populateWeatherData() {
        const response = await fetch('/api/weatherforecast');
        const data = await response.json();
        setForecasts(data);
    }
    
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
        updateLoginStatus(null);
    }

}

export default App;