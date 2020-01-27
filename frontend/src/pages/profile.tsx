import React, { useState } from "react";
import Button from "atoms/Button";
import { DootType } from "interfaces/enums/doot-type";
import TopicDootsService from "services/topics/topic-doots-service";

const ProfilePage: React.FC = () => {
    const [isLoading, setLoading] = useState(false);
    
    const onClick = async () => {
        setLoading(true);
        
        const temp = await TopicDootsService.post({
            userId:   1,
            topicId:  1,
            dootType: DootType.NotADoot
        })

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