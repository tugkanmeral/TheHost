function RequestButton(props) {
    return (
        <button
            type="button"
            className={props.classNames}
            style={props.style}
            onClick={props.onClick}
            disabled={props.isOnRequest}>
            {
                props.isOnRequest
                    ?
                    <div className="d-flex justify-content-center">
                        <div className="spinner-border spinner-border-sm" style={{ margin: "4px" }} role="status">
                        </div>
                    </div>
                    : props.children
            }
        </button>
    )
}

export default RequestButton