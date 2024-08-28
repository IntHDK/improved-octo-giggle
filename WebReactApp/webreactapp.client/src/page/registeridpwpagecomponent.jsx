import PropTypes from 'prop-types';
import { forwardRef, useEffect, useState } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { PostLogin, PostRegisterIDPW, GetRegisterIDPWIsAvailableUsername } from '../utils/identityrequest';

const RegisterIDPWPageComponent = forwardRef(function RegisterPageComponent(props, ref) {
    const [inputUsername, setInputUsername] = useState('');
    const [inputNickname, setInputNickname] = useState('');
    const [inputPassword, setInputPassword] = useState('');
    const [inputEmail, setInputEmail] = useState('');
    const [recentError, setRecentError] = useState('');

    let [searchParams] = useSearchParams();
    const navigate = useNavigate();
    let redirecturl = searchParams.get("redirecturl");
    if (typeof redirecturl != 'string') {
        redirecturl = '/';
    }

    function checkLoginStatusData(loginstatus) {
        props.loginstatuschecker();
        if (loginstatus.isLogin) {
            navigate(redirecturl);
        }
    }
    useEffect(() => {
        checkLoginStatusData(ref);
    }, []);

    const usernameChangeHandler = (e) => {
        setInputUsername(e.target.value);
    }
    const nicknameChangeHandler = (e) => {
        setInputNickname(e.target.value);
    }
    const passwordChangeHandler = (e) => {
        setInputPassword(e.target.value);
    }
    const emailChangeHandler = (e) => {
        setInputEmail(e.target.value);
    }

    const registerHandler = () => {
        GetRegisterIDPWIsAvailableUsername(inputUsername).then((v) => {
            if (v.isAvailable) {
                console.log(v);
                PostRegisterIDPW(inputUsername, inputNickname, inputEmail, inputPassword).then((registerresult) => {
                    console.log(registerresult);
                    if (registerresult.isSuccess) {
                        PostLogin(inputUsername, inputPassword).then((loginresult) => {
                            console.log(loginresult);
                            props.loginnotifier(loginresult.token, loginresult.expireAt);
                            console.log(navigate);
                            console.log(redirecturl);
                            navigate(redirecturl);
                        });
                    }
                });
            } else {
                setRecentError('사용할 수 없는 username입니다.');
            }
        });
    }
    const backHandler = () => {
        navigate(redirecturl);
    }

    var errorview;
    if (recentError != "") {
        errorview = <div className="row">
            <p>{recentError}</p>
        </div>
    } else {
        errorview = <div className="row">
        </div>
    }

    return (
        <div>
            <div className="row">
                <div className="col">
                    <h1>Register (Manual login)</h1>
                </div>
            </div>
            {errorview}
            <div className="row">
                <div className="col">
                    <label htmlFor="username">
                        Username
                    </label>
                    <input type="text" className="form-control" id="username" placeholder="Username" value={inputUsername} onChange={usernameChangeHandler} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <label htmlFor="password">
                        Password
                    </label>
                    <input type="password" className="form-control" id="password" value={inputPassword} onChange={passwordChangeHandler} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <label htmlFor="email">
                        Email
                    </label>
                    <input type="email" className="form-control" id="email" value={inputEmail} onChange={emailChangeHandler} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <label htmlFor="nickname">
                        Nickname
                    </label>
                    <input type="nickname" className="form-control" id="nickname" value={inputNickname} onChange={nicknameChangeHandler} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <button type="button" className="btn btn-primary" onClick={registerHandler}>Register</button>
                    <button type="button" className="btn btn-secondary" onClick={backHandler}>Back</button>
                </div>
            </div>
        </div>
    );
});
RegisterIDPWPageComponent.propTypes = {
    loginstatuschecker: PropTypes.func,
    loginnotifier: PropTypes.func
};

export default RegisterIDPWPageComponent;