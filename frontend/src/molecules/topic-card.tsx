import React from "react";
import Topic from "interfaces/topics/topic";
import { useAuthStoreState } from "hooks/auth-store-hooks";
import TopicDoot from "interfaces/topics/topic-doot";
import { DootType } from "interfaces/enums/doot-type";
import TopicDootsService from "services/topics/topic-doots-service";

const TopicCard: React.FC<{ topic: Topic, topicDoot?: TopicDoot, onDoot?: (topicId: number) => void }> = (props) => {
    const { topic, topicDoot } = props;

    const authState = useAuthStoreState(actions => actions);

    const dooted     = topicDoot != null && topicDoot.dootType != DootType.NotADoot;
    const upDooted   = dooted && topicDoot?.dootType == DootType.Updoot;
    const downDooted = dooted && topicDoot?.dootType == DootType.Downdoot;

    const doot = async (dootType: DootType) => {
        if (topicDoot != null)
        {
            await TopicDootsService.put({
                id:       topicDoot.id,
                dootType: dootType,
                topicId:  topic.id,
                userId:   1,
            })
        } else {
            await TopicDootsService.post({
                dootType: dootType,
                topicId:  topic.id,
                userId:   1,
            })
        }

        if (props.onDoot != null) {
            props.onDoot(topic.id);
        }
    }

    return (
        <div className="topic-card">
            <h1>{topic.title}</h1>
            <div className = "content">
                <p>
                    {topic.body}
                </p>
            </div>
            <ul>
                <li>
                    { // if
                        authState.isAuthenticated &&
                        !upDooted &&
                        <a href="#" onClick={() => doot(DootType.Updoot)}>Updoot</a>
                    }
                    {
                        authState.isAuthenticated &&
                        upDooted &&
                        <a href="#" onClick={() => doot(DootType.NotADoot)}>Remove Updoot</a>
                    }
                    <span>
                        {topic.updoots} updoots
                    </span>
                </li>
                <li>
                    { // if
                        authState.isAuthenticated &&
                        !downDooted &&
                        <a href="#" onClick={() => doot(DootType.Downdoot)}>Downdoot</a>
                    }
                    { // if
                        authState.isAuthenticated &&
                        downDooted &&
                        <a href="#" onClick={() => doot(DootType.NotADoot)}>Remove Downdoot</a>
                    }
                    <span>
                        {topic.downdoots} downdoots
                    </span>
                </li>
                <li>
                    { // if
                        authState.isAuthenticated &&
                        <a href="#">Comment</a>
                    }
                    <span>
                        0 comments
                    </span>
                </li>
            </ul>
        </div>
    );
}

export default TopicCard;