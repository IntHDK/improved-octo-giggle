import PropTypes from 'prop-types';
import { forwardRef } from 'react';
import { Link, createSearchParams, useLocation } from 'react-router-dom';

const LoginComponent = forwardRef(function LoginComponent(props, ref) {
    const location = useLocation();
    const loginredirect = "/login?" + createSearchParams({
        "redirecturl": location.pathname
    }).toString();
    const logoutredirect = "/logout?" + createSearchParams({
        "redirecturl": location.pathname
    }).toString();

    if (!ref.isLogin) {
        return (
            <div className="row">
                <div className="col text-right">
                    <Link to={loginredirect}><div className="btn btn-primary">Login</div></Link>
                </div>
            </div>
        );
    } else {
        return (
            <div className="row">
                <div className="col text-right">
                    You logged in with {ref.currentUsername}
                    <Link to={logoutredirect}><div className="btn btn-secondary">Logout</div></Link>
                </div>
            </div>

        );
    }
});

LoginComponent.propTypes = {
    logoutnotifier: PropTypes.func
};

export default LoginComponent;