import React, { useEffect } from "react";
import TopicCard from "molecules/topic-card";
import { TopicsContext } from "contexts/topics-store-context";
import { useStores } from "hooks/mobx-hook";
import { TopicDootsContext } from "contexts/topic-doots-store-context";
import { useObserver } from "mobx-react-lite";

const HomePage: React.FC = () => {
    const { topicsStore }     = useStores(TopicsContext);
    const { topicDootsStore } = useStores(TopicDootsContext);

    useEffect(() => {
        topicsStore.getTopics();
        topicDootsStore.getTopicDoots();
    }, [])

    return (
        useObserver(() => (
            <React.Fragment>
                {
                    topicsStore.topics.map((t) => {
                        const doot = topicDootsStore.topicDoots?.find(e => e.userId == 1 && e.topicId == t.id);

                        return (
                            <TopicCard
                                key       = {t.id}
                                topic     = {t}
                                topicDoot = {doot} />
                        )
                    })
                }
            </React.Fragment>
        ))
    );
}

export default HomePage;