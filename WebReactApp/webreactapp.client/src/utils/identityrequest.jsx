async function PostLogin(loginusername, loginpassword) {
    var response;
    var data;
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
            console.log(response);
            throw new Error();
        }
        data = await response.json();
        console.log(data);
        return data;

    } catch {
        console.log("bad");
        return null;
    }
}

async function GetRegisterIDPWIsAvailableUsername(username) {
    var response;
    var data;
    try {
        response = await fetch('/api/identity/register/idpw/isavailableusername/'+username, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            },
        });
        if (!response.ok) {
            console.log(response);
            throw new Error();
        }
        data = await response.json();
        console.log(data);
        return data;

    } catch {
        console.log("bad");
        return null;
    }
}

async function PostRegisterIDPW(username, nickname, email, password) {
    var response;
    var data;
    try {
        response = await fetch('/api/identity/register/idpw', {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                Username: username,
                Nickname: nickname,
                Email: email,
                Password: password
            }),
        });
        if (!response.ok) {
            console.log(response);
            throw new Error();
        }
        data = await response.json();
        console.log(data);
        return data;

    } catch {
        console.log("bad");
        return null;
    }
}

//export default PostLogin;
export { PostLogin, PostRegisterIDPW, GetRegisterIDPWIsAvailableUsername };