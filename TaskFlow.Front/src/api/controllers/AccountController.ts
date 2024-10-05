import LoginAttempt from "../../models/LoginAttempt";
import RegisterAttempt from "../../models/RegisterAttempt";
import requests from "../agent";

const accountController = {
    Login: async (loginAttempt: LoginAttempt) => {
        return await requests.postWithLoading("/Account/Login", loginAttempt);
    },
    Register: async (registerAttempt: RegisterAttempt) => {
        return await requests.post("/Account/Register", registerAttempt);
    },
    GetCurrentUser: async () => {
        return await requests.get("/Account/GetCurrentUser");
    },
    RefreshToken: async () => {
        return await requests.get("/Account/RefreshToken");
    },
};

export default accountController;
