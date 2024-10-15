import { makeAutoObservable } from "mobx";
import RequestError from "../models/common/RequestError";
import { ErrorTypes } from "../enums/ErrorTypes";

class ErrorStore {
    errorMap: Map<ErrorTypes, RequestError[]> = new Map();

    constructor() {
        makeAutoObservable(this);
    }

    addError(error: RequestError) {
        const errorType = error.errorType as ErrorTypes;
        const errors = this.errorMap.get(errorType) || [];

        if (
            errors.some(
                (existingError) => existingError.errorCode === error.errorCode
            )
        ) {
            return;
        }

        this.errorMap.set(errorType, [...errors, error]);

        if (errorType === ErrorTypes.GlobalError) {
            this.scheduleErrorRemoval(error, 6000);
        }
    }

    removeErrors(
        errorType: ErrorTypes,
        errorCode: string | undefined = undefined
    ) {
        const errors = this.errorMap.get(errorType) || [];
        this.errorMap.set(
            errorType,
            errors.filter(
                (error) =>
                    error.errorType !== errorType &&
                    (!errorCode || error.errorCode === errorCode)
            )
        );
    }

    addAuthError(message: string) {
        const newError: RequestError = {
            errorType: ErrorTypes.AuthenticationError,
            errorMessage: message,
            errorCode: "AuthError"
        }

        this.addError(newError);
    }

    scheduleErrorRemoval(error: RequestError, timeout: number) {
        setTimeout(() => {
            this.removeErrors(ErrorTypes.GlobalError, error.errorCode);
        }, timeout);
    }

    getAuthenticationErrors() {
        return this.errorMap.get(ErrorTypes.AuthenticationError) || [];
    }

    getGlobalErrors() {
        return this.errorMap.get(ErrorTypes.GlobalError) || [];
    }
}

const errorStore = new ErrorStore();
export default errorStore;
