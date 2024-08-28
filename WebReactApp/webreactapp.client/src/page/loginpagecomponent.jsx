import PropTypes from 'prop-types';
import { forwardRef, useEffect, useState } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { PostLogin } from '../utils/identityrequest';

const LoginPageComponent = forwardRef(function LoginedStatusComponent(props, ref) {
    const [loginInputUsername, setLoginInputUsername] = useState('');
    const [loginInputPassword, setLoginInputPassword] = useState('');
    let [searchParams] = useSearchParams();
    const navigate = useNavigate();

    let redirecturl = searchParams.get("redirecturl");
    if (typeof redirecturl != 'string') {
        redirecturl = '/';
    }

    async function postLogin(loginusername, loginpassword) {
        var data = PostLogin(loginusername, loginpassword);

        if (data != null) {
            if (data.isSuccess) {
                props.loginnotifier(data.token, data.expireAt);
                navigate(redirecturl);
            }
        }
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

    const emailChangeHandler = (e) => {
        setLoginInputUsername(e.target.value);
    }
    const passwordChangeHandler = (e) => {
        setLoginInputPassword(e.target.value);
    }
    const loginHandler = () => {
        postLogin(loginInputUsername, loginInputPassword);
    }
    const registerHandler = () => {
        navigate({
            pathname: '/register',
            search: searchParams.toString()
        });
    }

    return (
        <div>
            <div className="row">
                <div className="col">
                    <h1>Login</h1>
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <label htmlFor="loginform_username">
                        Username
                    </label>
                    <input type="text" className="form-control" id="loginform_username" placeholder="Username" value={loginInputUsername} onChange={emailChangeHandler} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <label htmlFor="loginform_password">
                        Password
                    </label>
                    <input type="password" className="form-control" id="loginform_password" value={loginInputPassword} onChange={passwordChangeHandler} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <button type="button" className="btn btn-primary" onClick={loginHandler}>Login</button>
                    <button type="button" className="btn btn-secondary" onClick={registerHandler}>Register</button>
                </div>
            </div>
        </div>
    );
});
LoginPageComponent.propTypes = {
    loginstatuschecker: PropTypes.func,
    loginnotifier: PropTypes.func
};
export default LoginPageComponent;