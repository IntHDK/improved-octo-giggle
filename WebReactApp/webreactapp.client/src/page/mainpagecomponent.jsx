import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

function MainPageComponent() {
    const [forecasts, setForecasts] = useState();
    async function populateWeatherData() {
        const response = await fetch('/api/weatherforecast');
        const data = await response.json();
        setForecasts(data);
    }

    useEffect(() => {
        populateWeatherData();
    }, []);

    const forecastcontext = forecasts === undefined
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
        <div>
            <div className="row">
                <div className="col">
                    채널참가
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <Link to="/chat/public">Public Chat</Link>
                </div>
                <div className="col">
                    <Link to="/chat/hourly">1분마다 메세지 보내는 채널</Link>
                </div>
                <div className="col">
                    <Link to="/chat/toggle">토글 상태 공유</Link>
                </div>
            </div>
            <div className="row">
                <h1 id="tableLabel">Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {forecastcontext}
            </div>
        </div>
    );
}

export default MainPageComponent;