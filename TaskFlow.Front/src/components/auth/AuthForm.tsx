import { Alert, Tab, Tablist } from "evergreen-ui";
import { useState } from "react";
import { observer } from "mobx-react";
import errorStore from "../../stores/ErrorStore";
import localStyles from "./AuthForm.module.css"
import { ErrorTypes } from "../../enums/ErrorTypes";
import LoginForm from "./login/LoginForm";
import RegisterForm from "./register/RegisterForm";

const authFormTabs = ["Login", "Register"];

enum AuthFormEnum {
    Login = 0,
    Register = 1
}

const AuthPageDelegator = (props: { selectedPage: number }) => {
    switch (props.selectedPage) {
        case AuthFormEnum.Login:
            return <LoginForm />
        case AuthFormEnum.Register:
            return <RegisterForm />
        default: return <>Not implemented</>
    }
}

const AuthForm = observer(() => {

    const authErrors = errorStore.getAuthenticationErrors();

    const [selectedTab, setSelectedTab] = useState(0);

    const handleTabChange = (tabIndex: number) => {
        errorStore.removeErrors(ErrorTypes.AuthenticationError);
        setSelectedTab(tabIndex);
    }

    return <>
        <div className={localStyles.authFormContainer}>
            <Tablist>
                {authFormTabs.map((tab, index) => (
                    <Tab isSelected={index === selectedTab} key={tab} onSelect={() => handleTabChange(index)}>
                        {tab}
                    </Tab>
                ))}
            </Tablist>

            <AuthPageDelegator selectedPage={selectedTab} />

            {authErrors.length > 0 && (
                <Alert intent="danger" >{authErrors[0].errorMessage}</Alert>
            )}
        </div>
    </>
})

export default AuthForm;