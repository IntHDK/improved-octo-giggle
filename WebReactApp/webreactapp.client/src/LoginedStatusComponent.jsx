import { forwardRef, useEffect, useState } from 'react';
/* eslint-disable react/prop-types */
const LoginedStatusComponent = forwardRef(function LoginedStatusComponent(props, ref) {
    const [isLogin, setIsLogin] = useState(false);
    const [loginInputEmail, setLoginInputEmail] = useState('');
    const [loginInputPassword, setLoginInputPassword] = useState('');
    const [currentEmail, setCurrentEmail] = useState('');

    function setLoginStatusData(loginstatus) {
        setIsLogin(loginstatus.isLogin);
        if (loginstatus.isLogin) {
            setCurrentEmail(loginstatus.currentEmail);
        }
    }

    useEffect(() => {
        setLoginStatusData(ref);
    }, []);

    const emailChangeHandler = (e) => {
        setLoginInputEmail(e.target.value);
    }
    const passwordChangeHandler = (e) => {
        setLoginInputPassword(e.target.value);
    }
    const loginHandler = () => {
        postLogin(loginInputEmail, loginInputPassword);
    }
    const logoutHandler = () => {
        props.logoutnotifier();
    }

    async function postLogin(loginusername, loginpassword) {
        var response;
        var data;
        console.log(loginusername + ":" + loginpassword);
        try {
            response = await fetch('/api/identity/login/idpw', {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    Username: loginusername,
                    Password: loginpassword
                }),
            });
            if (!response.ok) {
                throw new Error();
            }
            data = await response.json();
            console.log(data);
        } catch {
            console.log(response.status);
        }
        if (data.isSuccess) {
            props.loginnotifier(data.token, data.expireAt)
        }

    }

    const loginform =
        <div className="row">
            <div className="col-4">
                <label htmlFor="loginform_email">
                    Email
                </label>
                <input type="text" className="form-control" id="loginform_email" placeholder="email" value={loginInputEmail} onChange={emailChangeHandler} />
            </div>
            <div className="col-4">
                <label htmlFor="loginform_password">
                    Password
                </label>
                <input type="password" className="form-control" id="loginform_password" value={loginInputPassword} onChange={passwordChangeHandler} />
            </div>
            <div className="col-2">
                <button type="submit" className="btn form-control btn-primary" onClick={loginHandler}>Login</button>
            </div>
            <div className="col-2">
                <button type="button" className="btn form-control btn-secondary">Register</button>
            </div>
        </div>;

    if (!ref.isLogin) {
        return (
            <div>
                {loginform}
            </div>
        );
    }
    return (
        <div className="row">
            <div className="col-8">
                <p>You logged in with {ref.currentEmail}</p>
            </div>
            <div className="col-4">
                <button className="btn btn-secondary form-control" onClick={logoutHandler}>Logout</button>
            </div>
        </div>
        
    );
});

export default LoginedStatusComponent;