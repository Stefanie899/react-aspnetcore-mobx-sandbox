import React, { useState } from "react";
import Button from "atoms/Button";
import AuthenticationServices from "services/authentication/authentication-service";

const ProfilePage: React.FC = () => {
    const [isLoading, setLoading] = useState(false);
    
    const onClick = async () => {
        setLoading(true);
        
        const temp = await AuthenticationServices.get();

        console.debug(temp);

        setLoading(false);
    }

    return (
        <React.Fragment>
            <h1>Das Profile!</h1>
            <Button
                isLoading = { isLoading }
                onClick   = { onClick }
                text      = "Test User Thingy"/>
        </React.Fragment>
    );
}

export default ProfilePage;