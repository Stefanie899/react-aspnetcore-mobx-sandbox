import React from "react";
import Topic from "interfaces/topics/topic";
import TopicDoot from "interfaces/topics/topic-doot";
import { DootType } from "interfaces/enums/doot-type";
import { useStores } from "hooks/mobx-hook";
import { AuthContext } from "contexts/auth-store-context";
import { TopicDootsContext } from "contexts/topic-doots-store-context";
import { TopicsContext } from "contexts/topics-store-context";
import { useObserver } from "mobx-react-lite";

const TopicCard: React.FC<{ topic: Topic, topicDoot?: TopicDoot }> = (props) => {
    const { topic, topicDoot } = props;

    const { authStore } = useStores(AuthContext);
    const { topicsStore }     = useStores(TopicsContext);
    const { topicDootsStore } = useStores(TopicDootsContext);

    const dooted     = topicDoot != null && topicDoot.dootType != DootType.NotADoot;
    const upDooted   = dooted && topicDoot?.dootType == DootType.Updoot;
    const downDooted = dooted && topicDoot?.dootType == DootType.Downdoot;

    const doot = async (dootType: DootType) => {
        await topicDootsStore.addOrUpdateDoot({
            id:       topicDoot?.id,
            dootType: dootType,
            topicId:  topic.id,
            userId:   1,
        });

        topicsStore.getTopic(topic.id);
    }

    return (
        useObserver(() => (
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
                            authStore.isAuthenticated &&
                            !upDooted &&
                            <a href="#" onClick={() => doot(DootType.Updoot)}>Updoot</a>
                        }
                        {
                            authStore.isAuthenticated &&
                            upDooted &&
                            <a href="#" onClick={() => doot(DootType.NotADoot)}>Remove Updoot</a>
                        }
                        <span>
                            {topic.updoots} updoots
                        </span>
                    </li>
                    <li>
                        { // if
                            authStore.isAuthenticated &&
                            !downDooted &&
                            <a href="#" onClick={() => doot(DootType.Downdoot)}>Downdoot</a>
                        }
                        { // if
                            authStore.isAuthenticated &&
                            downDooted &&
                            <a href="#" onClick={() => doot(DootType.NotADoot)}>Remove Downdoot</a>
                        }
                        <span>
                            {topic.downdoots} downdoots
                        </span>
                    </li>
                    <li>
                        { // if
                            authStore.isAuthenticated &&
                            <a href="#">Comment</a>
                        }
                        <span>
                            0 comments
                        </span>
                    </li>
                </ul>
            </div>
        ))
    );
}

export default TopicCard;