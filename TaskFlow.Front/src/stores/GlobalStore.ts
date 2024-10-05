import { makeAutoObservable } from "mobx";
import AuthResult from "../models/AuthResult";

class GlobalStore {
    loggedInUser: AuthResult | undefined = undefined;
    pageIsLoading = 0;

    constructor() {
        makeAutoObservable(this);
    }

    setLoggedInUser(user: AuthResult | undefined) {
        this.loggedInUser = user;
    }

    incrementPageLoading() {
        this.pageIsLoading++;
    }

    decrementPageLoading() {
        this.pageIsLoading = Math.max(0, this.pageIsLoading - 1);
    }

    get isPageLoading() {
        return this.pageIsLoading > 0;
    }
}

const globalStore = new GlobalStore();
export default globalStore;
