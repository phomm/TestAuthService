function setFormMessage(formElement, type, message) {
    const messageElement = formElement.querySelector(".form__message");

    messageElement.innerHTML = message;
    messageElement.classList.remove("form__message--success", "form__message--error");
    messageElement.classList.add(`form__message--${type}`);
}

function warn(formElement) {
    setFormMessage(formElement, "error", "Passwords error, provide equal passwords");
}

function checkPasswordPair(formElement) {
    const signupPassword = document.getElementById("signupPassword").value;
    const signupConfirmPassword = document.getElementById("signupConfirmPassword").value;
    if (signupPassword === "" || signupConfirmPassword === "")
        return; 

    if (signupPassword !== signupConfirmPassword)
        warn(formElement);
    else
        setFormMessage(formElement, "success", "");
}

function apiPost(endpoint, body, successFn, errorFn, formElement) {
    let http = { method: 'POST', body: body, headers: new Headers({ 'content-type': 'application/json' }) };
    fetch(endpoint, http)
        .then(resp => resp.ok ? resp.json() : resp.status >= 500 ? resp.text() : resp.json().then(notOkResponse => { throw notOkResponse; }))
        .then(data => {
            setFormMessage(formElement, "success", successFn(data));
        })
        .catch(error => {
            setFormMessage(formElement, "error", errorFn(error));
        });
}

document.addEventListener("DOMContentLoaded", () => {
    const loginForm = document.querySelector("#login");
    const createAccountForm = document.querySelector("#createAccount");
    const requestPasswordForm = document.querySelector("#requestPassword");
    const linkLogins = document.querySelectorAll("#linkLogin");
    const linkCreateAccount = document.querySelector("#linkCreateAccount");
    const linkRequestPassword = document.querySelector("#linkRequestPassword");
    
    const links = [...linkLogins, linkCreateAccount, linkRequestPassword];
    const forms = [...Array.from(linkLogins, link => loginForm), createAccountForm, requestPasswordForm];
    console.log(links, forms);
    links.forEach(link => {
        link.addEventListener("click", e => {
            e.preventDefault();
            let formsToHide = forms.slice();
            let formsToShow = formsToHide.splice(links.indexOf(e.target), 1);
            console.log(e.target, formsToShow, formsToHide);
            formsToHide.forEach(form => form.classList.add("form--hidden"));
            formsToShow.forEach(form => form.classList.remove("form--hidden"));;
        });
    });
    
    createAccountForm.addEventListener("submit", e => {
        e.preventDefault();

        const signupUser = document.getElementById("signupUserName").value;
        const signupEmail = document.getElementById("signupEmail").value;
        const signupPassword = document.getElementById("signupPassword").value;
        const signupData = JSON.stringify({ profileName: signupUser, registrationEmail: signupEmail, password: signupPassword });

        apiPost('/api/users/add', signupData, data => "User created !", data => "User creation failed: " + data.message, createAccountForm);
    });

    requestPasswordForm.addEventListener("submit", e => {
        e.preventDefault();

        const requestPasswordEmail = document.getElementById("requestPasswordEmail").value;
        const requestPasswordData = JSON.stringify(requestPasswordEmail);

        apiPost('/api/users/requestpassword', requestPasswordData, data => "Success " + data.data,
            data => "User request for password failed: " + data.message, requestPasswordForm);
    });

    document.querySelectorAll("#signupPassword").forEach(inputElement => {
        inputElement.addEventListener("blur", e => {
            checkPasswordPair(createAccountForm);        
        });
    });

}); 