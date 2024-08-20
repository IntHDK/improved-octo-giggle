import PropTypes from 'prop-types';
import { forwardRef, useEffect } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';

const LogoutPageComponent = forwardRef(function LogoutPageComponent(props, ref) {
    let [searchParams] = useSearchParams();
    let redirecturl = searchParams.get("redirecturl");
    const navigate = useNavigate();
    if (typeof redirecturl != 'string') {
        redirecturl = '/';
    }
    function checkLoginStatusData(loginstatus) {
        if (loginstatus.isLogin) {
            props.logoutnotifier();
            navigate(redirecturl);
        }
    }
    useEffect(() => {
        checkLoginStatusData(ref);
    }, []);

    return (
        <div>
        </div>
    );
});
LogoutPageComponent.propTypes = {
    loginstatuschecker: PropTypes.func,
    logoutnotifier: PropTypes.func
};

export default LogoutPageComponent;