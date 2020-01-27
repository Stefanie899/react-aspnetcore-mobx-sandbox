import { DootType } from "interfaces/enums/doot-type";

export default interface TopicDoot {
    dootType: DootType;
    id?:      number;
    topicId:  number;
    userId:   number;
}