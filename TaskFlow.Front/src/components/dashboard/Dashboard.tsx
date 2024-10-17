import { Button } from "evergreen-ui";
import useAuth from "../../hooks/useAuth";
import localStyles from "./Dashboard.module.css"
import { useNavigate } from "react-router-dom";
import { useGlobalContext } from "../../context/Global.context";

export default function Dashboard() {

    const auth = useAuth();
    const navigate = useNavigate();
    const globalContext = useGlobalContext();

    const handleLogout = async () => {
        await auth.logout();
        navigate("/login");
    }

    return <div className={localStyles.dashboardContainer}>
        <span>
            YOU ARE LOGGED IN AS: {globalContext.loggedInUser?.userName}
        </span>
        <Button onClick={() => {
            handleLogout()
        }}>Logout</Button>
    </div>
}