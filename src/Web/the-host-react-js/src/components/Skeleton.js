import './Skeleton.css'

export default function Skeleton(props) {
    return(
        <span 
            className="loading" 
            style={{
                width: props.width, 
                height: props.height
            }}></span>
    )
}