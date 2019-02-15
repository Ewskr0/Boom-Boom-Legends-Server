import {CollisionsManager} from "./CollisionsManager";
import {Point2D} from "../Utils/Point2D";

const FULL_TILE = 785;
const EMPTY_TILE = 0;

class CollisionsTiles {
    @CollisionsManager.register(FULL_TILE)
    static checkFullCollision(destination: Point2D): boolean {
        return true
    }

    @CollisionsManager.register(EMPTY_TILE)
    static checkEmptyCollision(destination: Point2D): boolean {
        return false
    }
}

export {CollisionsTiles}